using Financial.Application.Contract.Transaction.Command;
using Financial.Application.Contract.WalletService.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Query.Contract.UI.UserPanel.Seller;
using Shared.Application;
using Shared.Application.Auth;
using Shared.Domain.Enums;
using Shop.Application.Commands;
using Shop.Application.Contract.Order.Query;
using Shop.Application.Contract.OrderSeller.Command;
using Shop.Application.Contract.Product.Query;
using Shop.Application.Contract.ProductCategory.Query;
using Shop.Application.Contract.ProductSell.Command;
using Shop.Application.Contract.Seller.Command;
using Shop.Application.Contract.Seller.Query;
using Store.Application.Contract.StoreProduct.Command;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.UserPanel.Controllers
{
    [IgnoreAntiforgeryToken]
    [Area("UserPanel")]
    [Route("/Profile/[controller]/[action]/{Id?}")]
    [Authorize]
    public class SellerController : Controller
    {
        private readonly ISellerCommands _sellerCommands;
        private readonly IAuthService _authService;
        private readonly ISellerUserPanelQueries _sellerUserPanelQueries;
        private readonly IProductSellCommands _productSellCommands;
        private readonly IProductCategoryQueries _productCategoryQueries;
        private readonly IProductQueries _productQueries;
        private readonly ISellerQueries _sellerQueries;
        private readonly IOrderQueries _orderQueries;
        private readonly IWalletCommands _walletCommands;
        private readonly IOrderSellerCommands _orderSellerCommands;
        private readonly IStoreProductCommands _storeProductCommands;
        private readonly ITransactionCommands _transactionCommands;
        private int _userId;

        public SellerController(ISellerCommands sellerCommands, IAuthService authService,
            ISellerUserPanelQueries sellerUserPanelQueries, IProductSellCommands productSellCommands, 
            IProductCategoryQueries productCategoryQueries, IProductQueries productQueries, 
            ISellerQueries sellerQueries, IOrderQueries orderQueries, IWalletCommands walletCommands, 
            IOrderSellerCommands orderSellerCommands, IStoreProductCommands storeProductCommands,
            ITransactionCommands transactionCommands)
        {
            _sellerCommands = sellerCommands;
            _authService = authService;
            _sellerUserPanelQueries = sellerUserPanelQueries;
            _productSellCommands = productSellCommands;
            _productCategoryQueries = productCategoryQueries;
            _productQueries = productQueries;
            _sellerQueries = sellerQueries;
            _orderQueries = orderQueries;
            _walletCommands = walletCommands;
            _orderSellerCommands = orderSellerCommands;
            _storeProductCommands = storeProductCommands;
            _transactionCommands = transactionCommands;
        }

        public async Task<IActionResult> MyShops(bool status)
        {
            TempData["success"] = status;
            var UserId = _authService.GetLoginUserId();
            var shops = await _sellerUserPanelQueries.GetSellersForUserPanel(UserId);
            return View(shops);
        }

        [HttpGet]
        public IActionResult RequestForSales()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RequestForSales(RequestForSelasCommandModel command)
        {
            if (!ModelState.IsValid)
                return View(command);

            var UserId = _authService.GetLoginUserId();
            var res = await _sellerCommands.RequestForSales(UserId, command);

            return RedirectToAction("Myshops", new { status = res.Success });

        }

        public async Task<IActionResult> EditRequestForSales(int Id)
        {
            if (Id < 1)
                return NotFound();


            var Request = await _sellerCommands.GetForEditRequestForSales(Id);
            if (Request == null)
                return RedirectToAction("MyShops");
            return View(Request);

        }
        [HttpPost]
        public async Task<IActionResult> EditRequestForSales(EditRequestForSelasCommandModel command)
        {
            if (!ModelState.IsValid)
                return View(command);

            var res = await _sellerCommands.EditRequestForSales(command);
            if (res.Success)
                return RedirectToAction("MyShops", new { status = true });

            TempData["error"] = res.Message;
            return View(command);
        }
        [HttpGet]
        public async Task<IActionResult> EditSeller(int Id)
        {
            if (Id < 1)
                return NotFound();
            var seller = await _sellerCommands.GetForEditSellerAsync(Id);
            if (seller == null)
                return NotFound();
            return View(seller);
        }
        [HttpPost]
        public async Task<IActionResult> EditSeller(EditSellerQueryModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var res = await _sellerCommands.SendSellerChangeRequests(model);
            if (res.Success)
            {
                TempData["success"] = "درخواست شما ارسال شد و پس از تایید تغییرات اعمال خواهد شد";
                return View(model);
            }

            TempData["Error"] = res.Message;
            return View(model);

        }
        public async Task<IActionResult> AddProductToShop(int Id)
        {

            var categories = await _productCategoryQueries.GetCategoriesForAddProduct();
            CreateProductSellCommandModel model = new()
            {
                categories = categories,
                SellerId = Id,

            };
            return PartialView("_AddProductToShopPatial", model);
        }
        [HttpPost]
        public async Task<JsonResult> AddProductToShop(CreateProductSellCommandModel model)
        {
            var res = await _productSellCommands.CreateAsync(model);
            return new JsonResult(new { success = res.Success, message = res.Message });
        }
        [HttpPost]
        public async Task<IActionResult> GetProductsForAddToShop([FromBody] List<int> categoryIds)
        {

            var products = await _productQueries.GetProductsForAddToShop(categoryIds);

            return Json(new
            {
                success = true,
                data = products.Select(p => new { id = p.Id, title = p.Title })
            });
        }
        public async Task<IActionResult> SellersProducts(int sellerId, int pageId = 1,
            int take = 5, int categoryId = 0, string filter = "")
        {
            var userId = _authService.GetLoginUserId();
            bool ok = await _sellerQueries.IsSellerForUser(userId, sellerId);
            if (!ok)
                return NotFound();
            var products = await _sellerUserPanelQueries.GetProductsForSellerAsync(sellerId, pageId, take, filter, categoryId);
            products.SellerId = sellerId;
            return View(products);
        }
        public async Task<JsonResult> ChangeProductActivation(int sellerId, int Id)
        {
            if (Id < 1)
                return new JsonResult(new { success = false, message = "شناسه نا معتبر!" });
            var res = await _productSellCommands.ActivationChangeAsync(sellerId, Id);
            return new JsonResult(new { success = res.Success, message = res.Message });
        }
        public async Task<IActionResult> EditProduct(int Id)
        {
            if (Id < 1)
                return NotFound();
            var productSell = await _productSellCommands.GetForEditAsync(Id);
            if (productSell == null)
                return NotFound();
            return PartialView("_EditProductSellPartial", productSell);
        }
        [HttpPost]
        public async Task<JsonResult> EditProduct(EditProductSellCommandModel model)
        {
            var res = await _productSellCommands.EditAsync(model);
            if (res.Success)
                return new JsonResult(new { success = true, message = "محصول مورد نظر با موفقیت ویرایش شد" });
            return new JsonResult(new { success = false, message = res.Message });

        }

        public async Task<JsonResult> DeleteProductSell(int Id)
        {
            var res = await _productSellCommands.DeleteAsync(Id);
            if (res.Success)
                return new JsonResult(new { success = true, title = "محصول با موفقیت از فروشگاه شما حذف شد" });
            return new JsonResult(new { success = false, title = res.Message });
        }

        public async Task<IActionResult> Orders(int sellerId)
        {
            if (sellerId <= 0)
                return NotFound();
            _userId = _authService.GetLoginUserId();

            var ok = await _sellerQueries.IsSellerForUser(_userId, sellerId);
            if (!ok)
                return NotFound();
            SellersOrdersPaging orders = await _sellerUserPanelQueries.GetSellersOrdersForUserPanelAsync(sellerId, OrderSellerStatus.همه, 0, 0, "");
            orders.SellerId = sellerId;
            return View(orders);
        }
        [HttpPost]
        public async Task<IActionResult> Orders(int sellerId, OrderSellerStatus status, int RefId, int OrderId, int PageId, string Filter = "")
        {
            if (sellerId <= 0)
                return NotFound();
            _userId = _authService.GetLoginUserId();

            var ok = await _sellerQueries.IsSellerForUser(_userId, sellerId);
            if (!ok)
                return NotFound();
            SellersOrdersPaging orders = await _sellerUserPanelQueries.GetSellersOrdersForUserPanelAsync(sellerId, status, RefId, PageId, Filter);

            return PartialView("_SellersOrdersListPartial", orders);
        }


        public async Task<IActionResult> OrderDetails(int SellerId, int OrderId)
        {
            if (SellerId <= 0 || OrderId <= 0)
                return NotFound();

            _userId = _authService.GetLoginUserId();
            var ok = await _sellerQueries.IsSellerForUser(_userId,SellerId);   
            if (!ok)
                return NotFound();
            var orderDetails = await _sellerUserPanelQueries.GetOrderDetailsForSellerAsync(SellerId, _userId, OrderId);
           

            return View(orderDetails);
        }
        [HttpPost]
        [Route("/Profile/Order/ChangeOrderSellerStatus")]
        public async Task<IActionResult> ChangeOrderSellerStatus(OrderSellerStatus status, int OrderId, int SellerId)
        {
            if (OrderId <= 0 || SellerId <= 0)
                return NotFound();

            var userId = _authService.GetLoginUserId();
            bool ok = await _sellerQueries.IsSellerForUser(userId, SellerId);
            if (!ok)
                return NotFound();


            if (status == OrderSellerStatus.لغو_شده_توسط_فروشنده)

            {
                try
                {
                    var order = await _orderQueries.GetFactorforordercancellation(OrderId, SellerId);
                    var NewTransation = new CreateTransacionCommandModel
                    {
                        Authority = "",
                        Description = " واریز وجه به مشتری بابت لغو سفارش توسط فروشنده",
                        Portal = TransactionPortal.کیف_پول,
                        Price = order.PaymentPrice,
                        TransactionFor = TransactionFor.Wallet,
                        TransactionSource = TransactionSource.بازگشت_ریز_فاکتور,
                        TransactionType = TransactionType.واریز,
                        TransationById = userId,
                        UserId = order.CustomerId,
                    };
                    var transaction = await _transactionCommands.CreateAsync(NewTransation);
                    long transactionId = Convert.ToInt64(transaction.Data);
                    var DepositRes = await _walletCommands.DepositAsync(order.CustomerId, order.PaymentPrice, transactionId);
                    if (DepositRes.Success)
                    {
                        var paymentRes = await _transactionCommands.Payment(TransactionStatus.موفق, transactionId, string.Empty);
                        await UpdateInventoryAfterOrdercancellationBySellerAsync(order);
                    }
                    else
                        return new JsonResult(new { success = false, message = "خطا در بازگشت وجه به مشتری" });
                }
                catch (Exception)
                {
                    return new JsonResult(new { success = false, message = "خطا در بازگشت وجه به مشتری" });

                }
            }
            OperationResult res = await _orderSellerCommands.ChangeOrderSellerStatusAsync(SellerId, OrderId, status);
            return new JsonResult(new { success = res.Success, message = res.Message });
        }

        private async Task UpdateInventoryAfterOrdercancellationBySellerAsync(FactorforordercancellationQueryModel model)
        {

            foreach (var item in model.Products)
            {

                var changeAmountRes = await _productSellCommands.EditProductSellAmountAsync(new EditProductSellAmountCommandModel
                {
                    count = item.Count,
                    SellId = item.ProductSellId,
                    Type = StoreProductType.افزایش,
                });
                if (changeAmountRes.Success)
                {

                    await _storeProductCommands.CreateAsync(new CreateStoreProductCommandModel
                    {
                        Count = item.Count,
                        ProdcutSellId = item.ProductSellId,
                        StoreProductType = StoreProductType.افزایش,
                        StoreId = model.SellerId
                    });

                }


            }

        }

    }
}
