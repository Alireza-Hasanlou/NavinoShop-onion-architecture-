using Dto.Payment;
using Dto.Response.Payment;
using Financial.Application.Contract.Transaction.Command;
using Financial.Application.Contract.Transaction.Query;
using Financial.Application.Contract.WalletService.Commands;
using Leaf.xNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NavinoShop.WebApplication.Utility;
using NavinoShop.WebApplication.Utility.ViewModels;
using Newtonsoft.Json;
using Query.Contract.UI.UserPanel.Wallet;
using Shared.Application.Auth;
using Shared.Domain.Enums;
using System.Net;
using System.Text;
using System.Text.Json;
using ZarinPal.Class;
using static System.Net.WebRequestMethods;
using static ZarinPal.Class.Payment;
using HttpStatusCode = Leaf.xNet.HttpStatusCode;

namespace NavinoShop.WebApplication.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Route("/Profile/[action]")]
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class WalletController : Controller
    {
        private readonly IWalletQueryService _walletQueryService;
        private readonly IWalletCommands _walletCommands;
        private readonly IAuthService _authService;
        private readonly ITransactionQueries _transactionQueries;
        private readonly ITransactionCommands _transactionCommands;
        private readonly SiteData _siteData;
        private int _userId;


        public WalletController(IWalletQueryService walletQueryService, IWalletCommands walletCommands,
            IAuthService authService, ITransactionQueries transactionQueries
            , ITransactionCommands transactionCommands, IOptions<SiteData> options)
        {
            _walletQueryService = walletQueryService;
            _walletCommands = walletCommands;
            _authService = authService;
            _transactionQueries = transactionQueries;
            _transactionCommands = transactionCommands;
            _siteData = options.Value;
        }


        [HttpGet]
        public IActionResult ChargeWallet() => PartialView("_ChargeWalletUserPanelPartila");

        [HttpPost]
        public async Task<IActionResult> ChargeWallet(int amount, string description, TransactionPortal portal)
        {
            if (amount < 1000)
            {
                ModelState.AddModelError("transactionAmountInput", "مبلغ تراکنش باید بیشتر 1000 تومان باشد");
                return View();
            }

            return portal switch
            {
                TransactionPortal.زرین_پال => await ProcessZarinPalPayment(amount),
                TransactionPortal.به_پرداخت_ملت => await ProcessMellatPayment(0, amount),//not Implimented
                TransactionPortal.سامان => await ProcessSamanPayment(0, amount),//not Implimented
                _ => RedirectToAction("Wallet")
            };
        }

        [Route("/Profile/Payment")]
        public async Task<IActionResult> Payment(string Authority, string Status)
        {
            if (string.IsNullOrEmpty(Authority) || string.IsNullOrEmpty(Status))
                return NotFound();

            var transaction = await _transactionQueries.GetTransationForPaymentByAuthorityAsync(Authority);
            if (transaction?.Id == 0)
                return NotFound();

            try
            {
                string url = "https://sandbox.zarinpal.com/pg/v4/payment/verify.json";
                var payload = new
                {
                    merchant_id = _siteData.ZarinPalMerchantId,
                    authority =transaction.Authority,
                    amount = transaction.Price
                };

                string jsonPayload = JsonConvert.SerializeObject(payload);
                ZarinPalVerificationResponse result = new();
                using (var httpRequest = new Leaf.xNet.HttpRequest())
                {
                    // تنظیم هدرها
                    httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36";
                    httpRequest.AddHeader("Accept", "application/json");
                    httpRequest.AddHeader("Content-Type", "application/json");
                    httpRequest.ConnectTimeout = 30000; // 30 ثانیه

                    // ارسال درخواست POST
                    var response =  httpRequest.Post(url, jsonPayload, "application/json");

                    // 4. دریافت پاسخ
                    string responseBody = response.ToString();

                    // 5. بررسی وضعیت HTTP
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception($"خطا در ارتباط با زرین‌پال: {response.StatusCode}");
                    }

                    // 6. دسریالایز کردن پاسخ
                     result = JsonConvert.DeserializeObject<ZarinPalVerificationResponse>(responseBody);

                    // 7. اعتبارسنجی پاسخ زرین‌پال
                    if (result == null)
                    {
                        throw new Exception("پاسخ دریافتی از زرین‌پال نامعتبر است");
                    }

                  
                }

                if (result.Status == 100 && transaction.Status != TransactionStatus.موفق)
                {
                    
                    var success = await ProcessWalletTransaction(transaction, result.RefId);

                    if (success)
                    {
                        return View(new PaymentStatusViewModel
                        {
                            Status = result.Status,
                            RefId = result.RefId.ToString(),
                            Message = $"پرداخت با موفقیت انجام شد. کد پیگیری: {result.RefId}"
                        });
                    }
                }

                await _transactionCommands.Payment(TransactionStatus.نا_موفق, transaction.Id, result.RefId.ToString());

                return View(new PaymentStatusViewModel
                {
                    Status = result?.Status ?? -1,
                    RefId = result.RefId.ToString(),
                    Message = "تراکنش ناموفق - در صورت کسر وجه از حساب شما، مبلغ حداکثر تا 72 ساعت به حساب شما باز خواهد گشت"
                });
            }
            catch
            {

                await _transactionCommands.Payment(TransactionStatus.نا_موفق, transaction.Id, null);

                return View(new PaymentStatusViewModel
                {
                    Status = -1,
                    RefId = "-",
                    Message = "خطا در ارتباط با درگاه پرداخت. لطفاً مجدداً تلاش کنید."
                });
            }
        }

        public async Task<IActionResult> Wallet()
        {
            var userId = _authService.GetLoginUserId();
            var model = await _walletQueryService.GetWalletForUserPanel(userId);
            return View(model);
        }

        public async Task<JsonResult> LoadTransaction(int pageId)
        {
            var userId = _authService.GetLoginUserId();
            var model = await _transactionQueries.GetTransactionsForUserAsync(pageId, userId, TransactionFor.Wallet);
            return Json(model);
        }


        #region Private Methods

        private async Task<IActionResult> ProcessZarinPalPayment(int amount)
        {
            try
            {
                _userId = _authService.GetLoginUserId();
                var mobile = _authService.GetLoginUserMobile();
                var email = _authService.GetLoginUserEmail();
                string transactionDescription = $"شارژ کیف پول از درگاه ";

                var callbackUrl = $"{_siteData.SiteUrl}Profile/Payment";
                var requestZarinPalUrl = "https://sandbox.zarinpal.com/pg/v4/payment/request.json";
                var request = new ZarinPalRequestModel
                {
                    mobile = mobile,
                    callback_url = callbackUrl,
                    description = transactionDescription,
                    email = email,
                    currency = "IRT",
                    amount = amount,
                    merchant_id = _siteData.ZarinPalMerchantId
                };

                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    string jsonData = System.Text.Json.JsonSerializer.Serialize(request);
                    byte[] requestData = Encoding.UTF8.GetBytes(jsonData);
                    byte[] responseData = client.UploadData(requestZarinPalUrl, "POST", requestData);
                    string responseString = Encoding.UTF8.GetString(responseData);
                    ZarinPalResponseModel response = System.Text.Json.JsonSerializer.Deserialize<ZarinPalResponseModel>(responseString);
                    if (response.data.code == 100 && response.data.message.ToLower() == "success")
                    {
                        var transactionResult = await _transactionCommands.CreateAsync(new CreateTransacionCommandModel
                        {
                            UserId = _userId,
                            Description = transactionDescription,
                            Portal = TransactionPortal.زرین_پال,
                            TransactionFor = TransactionFor.Wallet,
                            TransactionSource = TransactionSource.پرداخت_از_درگاه,
                            Price = amount,
                            TransactionType = TransactionType.واریز,
                            Authority = response.data.authority,
                            TransationById = _userId,
                        });

                        if (!transactionResult.Success)
                        {
                            ViewData["error"] = "خطا در ثبت درخواست پرداخت";
                            return RedirectToAction("Wallet");
                        }


                        var transactionId = Convert.ToInt64(transactionResult.Data);
                        string RedirectUrl = $"https://sandbox.zarinpal.com/pg/StartPay/{response.data.authority}";
                        return Redirect(RedirectUrl);
                    }
                    ViewData["error"] = "خطا در ارتباط با درگاه پرداخت. لطفاً مجدداً تلاش کنید";
                    return View();
                }
            }
            catch (Exception)
            {
                ViewData["error"] = "خطا در ارتباط با درگاه پرداخت. لطفاً مجدداً تلاش کنید";
                return View();
            }
        }

        private async Task<IActionResult> ProcessMellatPayment(long transactionId, int amount)
        {
            // TODO: پیاده‌سازی پرداخت به پرداخت ملت
            // برای فعلاً تراکنش را حذف کن
            await _transactionCommands.DeleteAsync(transactionId);
            TempData["Error"] = "درگاه به پرداخت ملت در حال پیاده‌سازی است";
            return RedirectToAction("Wallet");
        }

        private async Task<IActionResult> ProcessSamanPayment(long transactionId, int amount)
        {
            // TODO: پیاده‌سازی پرداخت سامان
            await _transactionCommands.DeleteAsync(transactionId);
            TempData["Error"] = "درگاه سامان در حال پیاده‌سازی است";
            return RedirectToAction("Wallet");
        }
        private async Task<bool> ProcessWalletTransaction(TransationViewModel transaction, long RefId)
        {
            var deposit = await _walletCommands.DepositAsync(transaction.UserId, transaction.Price, transaction.Id);
            if (deposit.Success)
            {
                await _transactionCommands.Payment(TransactionStatus.موفق, transaction.Id, RefId.ToString());
                return true;
            }
            else
            {

                await _transactionCommands.Payment(TransactionStatus.نا_موفق, transaction.Id, RefId.ToString());
                return false;
            }
        }

        #endregion
    }
}
