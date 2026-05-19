using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Query.Contract.UI.UserPanel.Seller;
using Shared.Application.Auth;
using Shop.Application.Contract.Product.Query;
using Shop.Application.Contract.ProductCategory.Query;
using Shop.Application.Contract.ProductSell.Command;
using Shop.Application.Contract.Seller.Command;
using Shop.Application.Contract.Seller.Query;
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

        public SellerController(ISellerCommands sellerCommands, IAuthService authService,
            ISellerUserPanelQueries sellerUserPanelQueries, IProductSellCommands productSellCommands,
            IProductCategoryQueries productCategoryQueries, IProductQueries productQueries, ISellerQueries sellerQueries)
        {
            _sellerCommands = sellerCommands;
            _authService = authService;
            _sellerUserPanelQueries = sellerUserPanelQueries;
            _productSellCommands = productSellCommands;
            _productCategoryQueries = productCategoryQueries;
            _productQueries = productQueries;
            _sellerQueries = sellerQueries;
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
            return View( seller);
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
        public async Task<IActionResult> SellersProducts(int sellerId, int pageId = 1, int take = 5, int categoryId = 0, string filter = "")
        {
            var userId = _authService.GetLoginUserId();
            bool ok = await _sellerQueries.IsSellerForUser(userId, sellerId);
            if (!ok)
                return NotFound();
            var products = await _productQueries.GetProductsForSellerAsync(sellerId, pageId, take, filter, categoryId);
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
    }
}
