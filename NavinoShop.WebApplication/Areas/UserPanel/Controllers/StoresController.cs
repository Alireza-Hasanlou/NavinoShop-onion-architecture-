using Microsoft.AspNetCore.Mvc;
using Query.Contract.UI.UserPanel.Seller;
using Query.Contract.UI.UserPanel.Stores;
using Shared.Application.Auth;
using Shop.Application.Contract.ProductSell.Command;
using Shop.Application.Contract.Seller.Query;
using Store.Application.Contract.Store.Command;
using Store.Application.Contract.StoreProduct.Command;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.UserPanel.Controllers
{
    [IgnoreAntiforgeryToken]
    [Area("UserPanel")]
    [Route("/Profile/[controller]/[action]/{Id?}")]
    public class StoresController : Controller
    {
        private readonly IStoreUserPanelQueryService _storeUserPanelQueryService;
        private readonly ISellerQueries _sellerQueries;
        private readonly IAuthService _authService;
        private readonly IStoreCommands _storeCommands;
        private readonly IStoreProductCommands _storeProductCommands;
        private readonly IProductSellCommands _productSellCommands;

        public StoresController(IStoreUserPanelQueryService storeUserPanelQueryService, ISellerQueries sellerQueries,
            IAuthService authService, IStoreCommands storeCommands, IStoreProductCommands storeProductCommands,
            IProductSellCommands productSellCommands)
        {
            _storeUserPanelQueryService = storeUserPanelQueryService;
            _sellerQueries = sellerQueries;
            _authService = authService;
            _storeCommands = storeCommands;
            _storeProductCommands = storeProductCommands;
            _productSellCommands = productSellCommands;
        }

        public async Task<IActionResult> MyStores()
        {
            var userId = _authService.GetLoginUserId();
            var stores = await _storeUserPanelQueryService.GetStores(userId);
            return View(stores);
        }

        [HttpGet]
        public IActionResult Create() => PartialView("_CreateStorePartialView");

        [HttpPost]
        public async Task<JsonResult> Create(CreateStoreCommandModel model)
        {
            var userId = _authService.GetLoginUserId();
            model.UserId = userId;
            var res = await _storeCommands.CreateAsync(model);
            if (res.Success)
                return new JsonResult(new { success = true, message = "انبار جدید با موفقیت ایجاد شد" });
            return new JsonResult(new { success = false, message = res.Message });
        }

        public async Task<IActionResult> GetUsersShop()
        {
            var userId = _authService.GetLoginUserId();
            var shops = await _sellerQueries.GetUsersShopAsync(userId);
            return Json(shops);

        }

        public async Task<IActionResult> Details(int storeId)
        {
            if (storeId < 1)
                return NotFound();
            var details = await _storeUserPanelQueryService.GetStoreDetails(storeId);
            if (details == null)
                return NotFound();
            return View(details);
        }

        public async Task<IActionResult> GetProductSellsForAddToStore(int sellerId)
        {
            if (sellerId < 1)
                return NotFound();
            var res = await _storeUserPanelQueryService.GetProductSellsForAddToStore(sellerId);
            return Json(res);

        }
        [HttpPost]
        public async Task<JsonResult> AddStoreProduct(CreateStoreProductCommandModel model)
        {
            var changeAmountRes = await _productSellCommands.EditProductSellAmountAsync(new EditProductSellAmountCommandModel
            {
                count = model.Count,
                SellId = model.ProdcutSellId,
                Type = model.StoreProductType,
            });
            if (changeAmountRes.Success)
            {
                await _storeProductCommands.CreateAsync(model);
                return new JsonResult(new { success = true, message = "عملیات موفق" });
            }
            return new JsonResult(new { success = false, message = changeAmountRes.Message });

        }
        public async Task<IActionResult> EditStoreDescription(int storeId)
        {
            if (storeId < 1)
                return NotFound();
            var store= await _storeCommands.GetForEditAsync(storeId);
            return PartialView("_EditStoreDescription", store);
        }

        [HttpPost]
        public async Task<JsonResult> EditStoreDescription(EditStoreCommandModel model)
        {
            var res = await _storeCommands.EditAsync(model);
            if (res.Success)
                return new JsonResult(new { success = true, message = "عملیات موفق" });
            return new JsonResult(new { success = false, message = res.Message });
        }
    }
}
