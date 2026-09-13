using AutoMapper;
using Discount.Application.Contract.OrderDiscounts.Command;
using Discount.Application.Contract.OrderDiscounts.Query;
using Financial.Application.Contract.Transaction.Command;
using Financial.Application.Contract.Transaction.Query;
using Financial.Application.Contract.WalletService.Commands;
using Financial.Application.Contract.WalletService.Query;
using Leaf.xNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NavinoShop.WebApplication.Utility;
using NavinoShop.WebApplication.Utility.ViewModels;
using Newtonsoft.Json;
using PostModule.Application.Contract.PostCalculate;
using PostModule.Application.Contract.PostQuery;
using Query.Contract.UI.Cart;
using Query.Contract.UI.UserPanel.Order;
using Query.Contract.UI.UserPanel.UserAddress;
using Shared.Application;
using Shared.Application.Auth;
using Shared.Domain.Enums;
using Shop.Application.Contract.Cart;
using Shop.Application.Contract.Order.Command;
using Shop.Application.Contract.OrderSeller.Command;
using Shop.Application.Contract.ProductSell.Command;
using Store.Application.Contract.StoreProduct.Command;
using System.Net;
using System.Text;
using System.Text.Json;
using Users.Application.Contract.UserAddressService.Query;

namespace NavinoShop.WebApplication.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Route("/Order/[action]")]
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class OrderController : Controller
    {

        private readonly IAuthService _authService;
        private readonly IOrderCommands _orderCommands;
        private readonly ICartUiQueryService _cartUiQueryService;
        private readonly IOrderDiscountsQueries _orderDiscountsQueries;
        private readonly IUserAddressUiQueryService _userAddressUiQueryService;
        private readonly IPostCalculateApplication _postCalculateApplication;
        private readonly IUserAddressQueryService _userAddressQueryService;
        private readonly IOrderUserPanelQueryService _orderUserPanelQuery;
        private readonly IOrderDiscountsCommands _orderDiscountsCommands;
        private readonly IOrderSellerCommands _orderSellerCommands;
        private readonly ITransactionQueries _transactionQueries;
        private readonly IStoreProductCommands _storeProductCommands;
        private readonly ITransactionCommands _transactionCommands;
        private readonly IWalletCommands _walletCommands;
        private readonly IProductSellCommands _productSellCommands;
        private readonly IWalletQueries _walletQueries;
        private readonly ICartCommands _cartCommands;
        private readonly IPostQuery _postQuery;
        private readonly SiteData _siteData;
        private readonly IMapper _mapper;
        private int _userId;

        public OrderController(IAuthService authService, IOrderCommands orderCommands, ICartUiQueryService cartUiQueryService,
            IOrderDiscountsQueries orderDiscountsQueries, IUserAddressUiQueryService userAddressUiQueryService,
            IPostCalculateApplication postCalculateApplication, IUserAddressQueryService userAddressQueryService,
            IOrderUserPanelQueryService orderUserPanelQuery, IOrderDiscountsCommands orderDiscountsCommands,
            IOrderSellerCommands orderSellerCommands, ITransactionQueries transactionQueries, IStoreProductCommands storeProductCommands, ITransactionCommands transactionCommands,
            IWalletCommands walletCommands, IProductSellCommands productSellCommands, IWalletQueries walletQueries, ICartCommands cartCommands, IPostQuery postQuery, IOptions<SiteData> siteData, IMapper mapper)

        {

            _authService = authService;
            _orderCommands = orderCommands;
            _cartUiQueryService = cartUiQueryService;
            _orderDiscountsQueries = orderDiscountsQueries;
            _userAddressUiQueryService = userAddressUiQueryService;
            _postCalculateApplication = postCalculateApplication;
            _userAddressQueryService = userAddressQueryService;
            _orderUserPanelQuery = orderUserPanelQuery;
            _orderDiscountsCommands = orderDiscountsCommands;
            _orderSellerCommands = orderSellerCommands;
            _transactionQueries = transactionQueries;
            _storeProductCommands = storeProductCommands;
            _transactionCommands = transactionCommands;
            _walletCommands = walletCommands;
            _productSellCommands = productSellCommands;
            _walletQueries = walletQueries;
            _cartCommands = cartCommands;
            _postQuery = postQuery;
            _siteData = siteData.Value;
            _mapper = mapper;
        }

        [Route("/checkout")]
        public async Task<IActionResult> Index()
        {

            _userId = _authService.GetLoginUserId();
            OrderUserPanelViewModel order = await _orderUserPanelQuery.GetOrderAsync(_userId);
            if (order != null)
                return View(order);

            List<CartUiQueryModel> cart = await _cartUiQueryService.GetAllAsync(_userId);
            if (!cart.Any())
                return NotFound();
            var shopCartVm = _mapper.Map<List<ShopCartViewModel>>(cart);
            UserAddressForOrderQueryModel UserDefaultAddress = await _userAddressQueryService.GetUserDefaultAddress(_userId);
            UpsertOrderAddressCommandModel UserDefaultAddressVm = new();
            if (UserDefaultAddress != null)
                UserDefaultAddressVm = _mapper.Map<UpsertOrderAddressCommandModel>(UserDefaultAddress);

            var UpsertRes = await _orderCommands.UpsertUserOrder(_userId, shopCartVm, UserDefaultAddressVm);
            if (UpsertRes.Success)
            {
                order = await _orderUserPanelQuery.GetOrderAsync(_userId);
                if (order.OrderSellers.Count < 1)
                    return NotFound();

                return View(order);
            }
            ViewData["Error"] = UpsertRes.Message;
            return View(new OrderUserPanelViewModel());



        }
        public async Task<IActionResult> ApplySellerDiscount(int SellerId, string Code)
        {
            OperationResultOrderDiscount result = await _orderDiscountsQueries.GetOrderSellerDiscountAsync(SellerId, Code);
            if (!result.Success)
                return new JsonResult(new { success = false, message = result.Message });

            _userId = _authService.GetLoginUserId();
            PricesAfterApplyDiscountDto applyRes = await _orderCommands.ApplySellerDiscountAsync(_userId, SellerId, result.Id, result.Percent, result.Title);


            if (applyRes.Success)
                return Json(applyRes);
            else
            {
                await _orderDiscountsCommands.MinusUseDiscountAsync(result.Id);
                return new JsonResult(new { success = false, message = applyRes.Message });
            }

        }
        public async Task<IActionResult> RemoveSellerDiscount(int SellerId)
        {
            _userId = _authService.GetLoginUserId();
            PricesAfterApplyDiscountDto RemoveRes = await _orderCommands.RemoveSellerDiscountAsync(_userId, SellerId);
            if (!RemoveRes.Success)
                await _orderDiscountsCommands.MinusUseDiscountAsync(RemoveRes.DiscountId);
            return Json(RemoveRes);
        }
        [HttpPost]
        public async Task<IActionResult> ApplyOrderDiscount(string Code)
        {
            OperationResultOrderDiscount result = await _orderDiscountsQueries.GetOrderSellerDiscountAsync(0, Code);
            if (!result.Success)
                return new JsonResult(new { success = false, message = result.Message });

            _userId = _authService.GetLoginUserId();
            PricesAfterApplyDiscountDto applyRes = await _orderCommands.ApplyOrderDiscountAsync(_userId, result.Id, result.Percent, result.Title);
            // قبل از ارسال به View

            if (applyRes.Success)
                return Json(applyRes);
            else
            {
                await _orderDiscountsCommands.MinusUseDiscountAsync(result.Id);
                return new JsonResult(new { success = false, message = applyRes.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> RemoveOrderDiscount(int OrderId)
        {
            _userId = _authService.GetLoginUserId();
            PricesAfterApplyDiscountDto RemoveRes = await _orderCommands.RemoveOrderDiscountAsync(_userId, OrderId);
            if (!RemoveRes.Success)
                await _orderDiscountsCommands.MinusUseDiscountAsync(RemoveRes.DiscountId);
            return Json(RemoveRes);
        }
        public async Task<IActionResult> GetUserAddresses()
        {
            _userId = _authService.GetLoginUserId();

            var UserAddresses = await _userAddressUiQueryService.GetUserAddressesAsync(_userId);
            return Json(UserAddresses);

        }
        public async Task<IActionResult> SelectOrderAddress(int addressId)
        {
            if (addressId < 0)
                return NotFound();
            UserAddressForOrderQueryModel UserAddress = await _userAddressQueryService.GetByIdAsync(addressId);
            if (UserAddress == null)
                return new JsonResult(new { success = false, message = "آدرس مورد نظر یافت نشد " });
            _userId = _authService.GetLoginUserId();
            var UserAddressModel = _mapper.Map<UpsertOrderAddressCommandModel>(UserAddress);
            OperationResult result = await _orderCommands.UpsertOrderAddressAsync(_userId, UserAddressModel);
            return new JsonResult(new { success = result.Success, message = result.Message });
        }
        public async Task<IActionResult> CalculatePostPrice(int ordersellerId)
        {
            var posts = new List<PostPriceResponseModel>();
            _userId = _authService.GetLoginUserId();
            var order = await _orderUserPanelQuery.GetOrderAsync(_userId);
            if (order == null)
                return new JsonResult(new { success = false, message = "خطا در دریافت روش های ارسال لطفا مجددا تلاش کنید" });
            else
            {
                if (order.OrderAddressId == 0 || order.OrderAddressId == null)
                    return new JsonResult(new { success = false, message = "لطفا جهت دریافت روش های ارسال یک آدرس اضافه کنید" });
                else
                {
                    var orderseller = order.OrderSellers.FirstOrDefault(x => x.Id == ordersellerId);
                    if (orderseller == null)
                        return new JsonResult(new { success = false, message = "خطا در یافتن فروشنده" });

                    int destinationCityId = await _orderUserPanelQuery.GetUserCityAsync(_userId);
                    if (destinationCityId == 0) return new JsonResult(new { success = false, message = "شهر مبدا یافت نشد" });
                    else
                    {
                        int weight = await _orderUserPanelQuery.CalculateOrdersellerWeightAsync(ordersellerId);
                        if (weight == 0) return new JsonResult(new { success = false, message = "خطا در بارگذاری وزن محصولات" });
                        else
                        {
                            posts = await _postCalculateApplication.CalculatePost(new PostPriceRequestModel
                            {
                                DestinationCityId = destinationCityId,
                                SourceCityId = orderseller.SellerCityId,
                                Weight = weight
                            });

                        }

                    }
                }
            }

            return Json(posts);
        }
        public async Task<IActionResult> UpdateShippingMethod([FromBody] AddPostToSellerDto addPostToSellerDto)
        {
            if (addPostToSellerDto.orderSellerId <= 0
                || addPostToSellerDto.postPrice <= 0
                || addPostToSellerDto.orderId <= 0
                || addPostToSellerDto.postId <= 0)
                return new JsonResult(new { success = false, message = "داده نا معتبر" });

            bool exsitPost = await _postQuery.IsExistPostAsync(addPostToSellerDto.postId);
            if (!exsitPost) return new JsonResult(new { success = false, message = "داده نامعتبر!" });
            _userId = _authService.GetLoginUserId();
            addPostToSellerDto = addPostToSellerDto with { userId = _userId };
            PricesAfterAddPost res = await _orderSellerCommands.AddPostToSellerAsync(addPostToSellerDto);
            return Json(res);

        }
        public async Task<IActionResult> FactorPayment(OrderPayment orderPayment, TransactionPortal portal)
        {
            _userId = _authService.GetLoginUserId();
            OrderUserPanelViewModel openOrder = await _orderUserPanelQuery.GetOrderAsync(_userId);
            if (openOrder == null)
                return new JsonResult(new { success = false, message = "خطا در پرداخت لطفا صفحه را مجددا بارگذاری کنید و سپس دوباره امتحان کنید" });

            foreach (var item in openOrder.OrderSellers)
            {
                if (string.IsNullOrEmpty(item.PostTitle))
                    return new JsonResult(new { success = false, message = "لطفا برای همه فروشنده ها روش ارسال را انتخاب کنید" });
            }
            var res = await _orderCommands.SetOrderPaymentType(orderPayment, _userId);
            if (!res.Success)
                return new JsonResult(new { success = false, message = "خطا در پرداخت لطفا صفحه را مجددا بارگذاری کنید و سپس دوباره امتحان کنید" });
            if (orderPayment == 0) // 0 = پرداخت از درگاه
            {

                return portal switch
                {
                    TransactionPortal.زرین_پال => await ProcessZarinPalPayment(openOrder, openOrder.PaymentPrice),
                    TransactionPortal.به_پرداخت_ملت => await ProcessMellatPayment(0, openOrder.PaymentPrice),
                    TransactionPortal.سامان => await ProcessSamanPayment(0, openOrder.PaymentPrice),
                    _ => RedirectToAction("checkout")
                };
            }
            else // کیف پول 
            {
                if (!await _walletQueries.WalletHasAmountAsync(_userId, openOrder.PaymentPrice))
                    return new JsonResult(new
                    {
                        success = false,
                        message = "مبلغ کیف پول شما کافی نیست " +
                        "لطفا از قسمت کیف پول در پروفایلتان ابتدا کیف پول خود را شارژ کنید سپس اقدارم به پرداخت کنید "
                    });


                var transactionResult = await _transactionCommands.CreateAsync(new CreateTransacionCommandModel
                {
                    UserId = _userId,
                    Description = $"پرداخت با کیف پول با شماره فاکتور {openOrder.OrderId}",
                    Portal = TransactionPortal.کیف_پول,
                    TransactionFor = TransactionFor.Wallet,
                    TransactionSource = TransactionSource.خرید_از_سایت,
                    Price = openOrder.PaymentPrice,
                    TransactionType = TransactionType.برداشت,
                    Authority = "",
                    TransationById = _userId,
                });

                if (!transactionResult.Success)
                    return new JsonResult(new { success = false, message = "خطا در ثبت درخواست پرداخت" });

                var SetOrderPaymentres = await _orderCommands.SetOrderPaymentType(orderPayment, _userId);
                if (!SetOrderPaymentres.Success)
                    return new JsonResult(new { success = false, message = "خطا در پرداخت لطفا صفحه را مجددا بارگذاری کنید و سپس دوباره امتحان کنید" });

                var transactionId = Convert.ToInt64(transactionResult.Data);
                var transaction = await _transactionQueries.GetTransationForPaymentByIdAsync(transactionId);
                if (transaction?.Id == 0)
                    return NotFound();

                var transactionRes = await ProcessWalletTransaction(transaction , openOrder);
                if (!transactionRes.Success)
                    return new JsonResult(new { success = false, message = transactionRes.Message });

                return new JsonResult(new { success = true, message = "پرداخت از کیف پول شما با موفقیت انجام شد ", redirectUrl = "/Profile/Wallet" });

            }

        }
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
                    authority = transaction.Authority,
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
                    var response = httpRequest.Post(url, jsonPayload, "application/json");

                    // 4. دریافت پاسخ
                    string responseBody = response.ToString();

                    // 5. بررسی وضعیت HTTP
                    if (response.StatusCode != Leaf.xNet.HttpStatusCode.OK)
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

                    var paymentRes = await _transactionCommands.Payment(TransactionStatus.موفق, transaction.Id, result.RefId.ToString());

                    if (paymentRes.Success)
                    {
                        return View("PaymentResultView", new PaymentStatusViewModel
                        {
                            Status = result.Status,
                            RefId = result.RefId.ToString(),
                            Message = $"پرداخت با موفقیت انجام شد. کد پیگیری: {result.RefId}"
                        });
                    }

                }

                await _transactionCommands.Payment(TransactionStatus.نا_موفق, transaction.Id, result.RefId.ToString());

                return View("PaymentResultView", new PaymentStatusViewModel
                {
                    Status = result?.Status ?? -1,
                    RefId = result.RefId.ToString(),
                    Message = "تراکنش ناموفق - در صورت کسر وجه از حساب شما، مبلغ حداکثر تا 72 ساعت به حساب شما باز خواهد گشت"
                });
            }
            catch
            {

                await _transactionCommands.Payment(TransactionStatus.نا_موفق, transaction.Id, null);

                return View("PaymentResultView", new PaymentStatusViewModel
                {
                    Status = -1,
                    RefId = "-",
                    Message = "خطا در ارتباط با درگاه پرداخت. لطفاً مجدداً تلاش کنید."
                });
            }
        }




        #region PrivateMethod
        private async Task<IActionResult> ProcessZarinPalPayment(OrderUserPanelViewModel openOrder, int amount)
        {
            try
            {
                var mobile = _authService.GetLoginUserMobile();
                var email = _authService.GetLoginUserEmail();
                string transactionDescription = $"پرداخت از درگاه با شماره فاکتور {openOrder.OrderId}";

                var callbackUrl = $"{_siteData.SiteUrl}Order/Payment";
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
                            TransactionFor = TransactionFor.Order,
                            TransactionSource = TransactionSource.پرداخت_از_درگاه,
                            Price = amount,
                            TransactionType = TransactionType.واریز,
                            Authority = response.data.authority,
                            TransationById = _userId,
                        });

                        if (!transactionResult.Success)
                            return new JsonResult(new { success = false, message = "خطا در ثبت درخواست پرداخت" });

                        var transactionId = Convert.ToInt64(transactionResult.Data);
                        string RedirectUrl = $"https://sandbox.zarinpal.com/pg/StartPay/{response.data.authority}";
                        OperationResult finalizeResult = await _orderCommands.FinalizePaymentAsync(_userId);
                        if (finalizeResult.Success)
                        {
                            await _cartCommands.ClearCartAsync(_userId);
                            await UpdateInventoryAfterPaymentAsync(openOrder);
                        }

                        else
                            return new JsonResult(new { success = false, message = finalizeResult.Message });

                        return new JsonResult(new { success = true, redirectUrl = RedirectUrl });
                    }
                    return new JsonResult(new { success = false, message = "خطا در ارتباط با درگاه پرداخت. لطفاً مجدداً تلاش کنید" });
                }
            }
            catch (Exception)
            {
                return new JsonResult(new { success = false, message = "خطا در ارتباط با درگاه پرداخت. لطفاً مجدداً تلاش کنید" });
            }
        }

        private async Task<IActionResult> ProcessMellatPayment(long transactionId, int amount)
        {

            await _transactionCommands.DeleteAsync(transactionId);
            TempData["Error"] = "درگاه به پرداخت ملت در حال پیاده‌سازی است";
            return RedirectToAction("Wallet");
        }
        private async Task<IActionResult> ProcessSamanPayment(long transactionId, int amount)
        {

            await _transactionCommands.DeleteAsync(transactionId);
            TempData["Error"] = "درگاه سامان در حال پیاده‌سازی است";
            return RedirectToAction("Wallet");
        }
        private async Task<OperationResult> ProcessWalletTransaction(TransationViewModel transaction , OrderUserPanelViewModel openOrder)
        {
            var deposit = await _walletCommands.WithdrawAsync(transaction.UserId, transaction.Price, transaction.Id);
            if (deposit.Success)
            {

                var paymentRes = await _transactionCommands.Payment(TransactionStatus.موفق, transaction.Id, "");
                if (paymentRes.Success)
                {
                    OperationResult finalizeResult = await _orderCommands.FinalizePaymentAsync(_userId);
                    if (finalizeResult.Success)
                    {
                        await _cartCommands.ClearCartAsync(_userId);
                        await UpdateInventoryAfterPaymentAsync(openOrder);
                        return new(true);
                    }
                    return new(false, finalizeResult.Message);
                }
                return new(false, paymentRes.Message);
            }
            else
            {

                var paymentRes = await _transactionCommands.Payment(TransactionStatus.نا_موفق, transaction.Id, "");
                if (paymentRes.Success)
                    return new(true);
                return new(false, paymentRes.Message);
            }
        }
        private async Task UpdateInventoryAfterPaymentAsync(OrderUserPanelViewModel openOrder)
        {

            foreach (var orderSeller in openOrder.OrderSellers)
            {
                foreach (var item in orderSeller.Items)
                {
                    var changeAmountRes = await _productSellCommands.EditProductSellAmountAsync(new EditProductSellAmountCommandModel
                    {
                        count = item.Quantity,
                        SellId = item.ProductSellId,
                        Type = StoreProductType.کاهش,
                    });
                    if (changeAmountRes.Success)
                    {

                         await _storeProductCommands.CreateAsync(new CreateStoreProductCommandModel
                        {
                            Count = item.Quantity,
                            ProdcutSellId = item.ProductSellId,
                            StoreProductType = StoreProductType.کاهش,
                            StoreId = orderSeller.SellerId
                        });

                    }
                }
            }

        }
        #endregion
    }
}
