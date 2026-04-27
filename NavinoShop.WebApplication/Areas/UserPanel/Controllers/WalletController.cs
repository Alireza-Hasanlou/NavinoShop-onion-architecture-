using Dto.Payment;
using Dto.Response.Payment;
using Financial.Application.Contract.Transaction.Command;
using Financial.Application.Contract.Transaction.Query;
using Financial.Application.Contract.WalletService.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NavinoShop.WebApplication.Utility;
using NavinoShop.WebApplication.Utility.ViewModels;
using Query.Contract.UI.UserPanel.Wallet;
using Shared.Application.Auth;
using Shared.Domain.Enums;
using System.Threading.Tasks;
using ZarinPal.Class;

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
        private readonly Payment _payment;
        private readonly SiteData _siteData;


        public WalletController(IWalletQueryService walletQueryService, IWalletCommands walletCommands,
            IAuthService authService, ITransactionQueries transactionQueries
            , ITransactionCommands transactionCommands, IOptions<SiteData> options)
        {
            _walletQueryService = walletQueryService;
            _walletCommands = walletCommands;
            _authService = authService;
            _transactionQueries = transactionQueries;
            _transactionCommands = transactionCommands;
            var expose = new Expose();
            _payment = expose.CreatePayment();
            _siteData = options.Value;
        }

        public async Task<IActionResult> ChargeWallet(int amount, string description, TransactionPortal portal)
        {
            var userId = _authService.GetLoginUserId();
            var transactionResult = await _transactionCommands.CreateAsync(new CreateTransacionCommandModel
            {
                UserId = userId,
                Description = description,
                Portal = portal,
                TransactionFor = TransactionFor.Wallet,
                TransactionSource = TransactionSource.پرداخت_از_درگاه,
                Price = amount,
                TransactionType = TransactionType.واریز,
                Authority = "",
                TransationById = userId
            });

            if (!transactionResult.Success)
            {
                TempData["Error"] = "خطا در ثبت درخواست پرداخت";
                return RedirectToAction("Wallet");
            }

            var transactionId = Convert.ToInt64(transactionResult.Data);

            return portal switch
            {
                TransactionPortal.زرین_پال => await ProcessZarinPalPayment(transactionId, amount, description),
                TransactionPortal.به_پرداخت_ملت => await ProcessMellatPayment(transactionId, amount),
                TransactionPortal.سامان => await ProcessSamanPayment(transactionId, amount),
                _ => RedirectToAction("Wallet")
            };
        }

        [Route("/Profile/[action]/{transationId}/{authority}/{status}")]
        public async Task<IActionResult> Payment(long transactionId, string authority, string status)
        {
            if (transactionId < 1 || string.IsNullOrEmpty(authority) || string.IsNullOrEmpty(status))
                return NotFound();

            var transaction = await _transactionQueries.GetTransationForPayment(transactionId);
            if (transaction?.Id == 0)
                return NotFound();

            try
            {
                var Verification = await _payment.Verification(new DtoVerification
                {
                    Amount = transaction.Price,
                    MerchantId = _siteData.ZarinPalMerchantId,
                    Authority = authority
                }, ZarinPal.Class.Payment.Mode.sandbox);

                if (Verification.Status == 100 && transaction.Status != TransactionStatus.موفق)
                {
                    var success = transaction.TransactionFor switch
                    {
                        TransactionFor.Wallet => await ProcessWalletTransaction(transaction, Verification.RefId),
                        TransactionFor.Order => await ProcessDefaultTransaction(transaction, Verification.RefId),
                        TransactionFor.PostOrder => await ProcessDefaultTransaction(transaction, Verification.RefId),
                        _ => false
                    };

                    if (success)
                    {
                        return View(new PaymentStatusViewModel
                        {
                            Status = Verification.Status,
                            RefId = Verification.RefId,
                            Message = $"پرداخت با موفقیت انجام شد. کد پیگیری: {Verification.RefId}"
                        });
                    }
                }

                await _transactionCommands.Payment(TransactionStatus.نا_موفق, transaction.Id, Verification.RefId.ToString());

                return View(new PaymentStatusViewModel
                {
                    Status = Verification?.Status ?? -1,
                    RefId = Verification.RefId,
                    Message = "تراکنش ناموفق - در صورت کسر وجه از حساب شما، مبلغ حداکثر تا 72 ساعت به حساب شما باز خواهد گشت"
                });
            }
            catch
            {

                await _transactionCommands.Payment(TransactionStatus.نا_موفق, transaction.Id, null);

                return View(new PaymentStatusViewModel
                {
                    Status = -1,
                    RefId = 0,
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

        private async Task<IActionResult> ProcessZarinPalPayment(long transactionId, int amount, string description)
        {
            try
            {
                var mobile = _authService.GetLoginUserMobile();
                var email = _authService.GetLoginUserEmail();

                var callbackUrl = $"{_siteData.SiteUrl}Profile/Payment/{transactionId}";

                var result = await _payment.Request(new DtoRequest
                {
                    Mobile = mobile,
                    CallbackUrl = callbackUrl,
                    Description = description,
                    Email = email,
                    Amount = amount,
                    MerchantId = _siteData.ZarinPalMerchantId,
                }, ZarinPal.Class.Payment.Mode.sandbox);

                if (result.Status == 100)
                {
                    var paymentUrl = _siteData.UseSandbox
                        ? $"https://sandbox.zarinpal.com/pg/StartPay/{result.Authority}"
                        : $"https://www.zarinpal.com/pg/StartPay/{result.Authority}";

                    return Redirect(paymentUrl);
                }

                await _transactionCommands.DeleteAsync(transactionId);
                TempData["Error"] = $"خطا در ارتباط با درگاه پرداخت: کد خطا {result.Status}";
                return RedirectToAction("Wallet");
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "خطا در پرداخت زرین‌پال برای تراکنش {TransactionId}", transactionId);

                await _transactionCommands.DeleteAsync(transactionId);
                TempData["Error"] = "خطا در ارتباط با درگاه پرداخت. لطفاً مجدداً تلاش کنید.";
                return RedirectToAction("Wallet");
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
        private async Task<bool> ProcessWalletTransaction(TransationViewModel transaction, int RefId)
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
        private async Task<bool> ProcessDefaultTransaction(TransationViewModel transaction, int RefId)
        {
            await _transactionCommands.Payment(TransactionStatus.موفق, transaction.Id, RefId.ToString());
            return true;
        }
        #endregion
    }
}
