using Discount.Application.Contract.OrderDiscounts.Command;
using Discount.Application.Contract.OrderDiscounts.Query;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Auth;
using Shared.Domain.Enums;
using Shop.Application.Contract.Seller.Query;
using Shop.Domain.SellerAgg;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [IgnoreAntiforgeryToken]
    [Route("/Profile/[controller]/[action]/{Id?}")]
    public class OrderDiscountsController : Controller
    {

        private readonly IOrderDiscountsQueries _orderDiscountsQueries;
        private readonly IOrderDiscountsCommands _orderDiscountsCommands;
        private readonly IAuthService _authService;
        private ISellerQueries _sellerQueries;


        public OrderDiscountsController(IOrderDiscountsQueries orderDiscountsQueries, IAuthService authService,
            ISellerQueries sellerQueries, IOrderDiscountsCommands orderDiscountsCommands)
        {
            _orderDiscountsQueries = orderDiscountsQueries;
            _authService = authService;
            _sellerQueries = sellerQueries;
            _orderDiscountsCommands = orderDiscountsCommands;
        }

        public async Task<IActionResult> Index(int Id) //SellerId
        {
            if (Id < 1)
                return NotFound();
            var userId = _authService.GetLoginUserId();
            bool ok = await _sellerQueries.IsSellerForUser(userId, Id);
            if (!ok)
                return NotFound();
            var discounts = await _orderDiscountsQueries.GeAllAsync(Id, OrderDiscountType.OrderSeller);
            ViewData["SellerId"] = Id;
            ViewData["SellerTitle"] = _sellerQueries.GetSellerTitle(Id);
            return View(discounts);
        }

        public async Task<IActionResult> Create(int Id)//SellerId
        {
            if (Id < 1)
                return NotFound();
            var userId = _authService.GetLoginUserId();
            bool ok = await _sellerQueries.IsSellerForUser(userId, Id);
            if (!ok)
                return NotFound();
            return PartialView("_CreateOrderSellerDiscountPartial", Id);
        }
        [HttpPost]
        public async Task<IActionResult> Create(UpsertOrderDiscountCommandModel model)
        {
            if (model.ShopId < 1)
                return Json(new { success = false, message = "شناسه فروشگاه معتبر نیست" });
            var userId = _authService.GetLoginUserId();
            bool ok = await _sellerQueries.IsSellerForUser(userId, model.ShopId);
            if (!ok) return Json(new { success = false, message = "شناسه فروشگاه معتبر نیست" });

            model.type = OrderDiscountType.OrderSeller;
            var result = await _orderDiscountsCommands.CreateOrderDiscountAsync(model);
            return Json(new { success = result.Success, message = result.Message });
        }

        public async Task<IActionResult> Edit(int DiscountId)
        {
            if (DiscountId < 1)
                return new JsonResult(new { success = false, message = "شناسه نامعتبر" });
            var Discount = await _orderDiscountsCommands.GetForEditAsync(DiscountId);
            return PartialView("_EditOrderSellerDiscountPartial", Discount);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int DiscountId, UpsertOrderDiscountCommandModel model)
        {
            if (DiscountId != model.Id || DiscountId == 0 || model.Id == 0)
                return new JsonResult(new { success = false, message = "شناسه نامعتبر" });
            model.type = OrderDiscountType.Order;
            var result = await _orderDiscountsCommands.EditOrderDiscountAsync(model);
            return new JsonResult(new { result.Success, result.Message });
        }

        public async Task<IActionResult> Delete(int Id)
        {

            var result = await _orderDiscountsCommands.DeleteAsync(Id);
            return new JsonResult(new { success = result.Success, errors = result.Message });
        }

        public async Task<IActionResult> ExpiredDiscounts(int Id)//ShopId
        {
            if (Id < 1)
                return NotFound();
            var ExpiredDiscounts = await _orderDiscountsQueries.GeAllExpiredDiscountsAsync(Id, OrderDiscountType.OrderSeller);
            ViewData["SellerId"] = Id;
            ViewData["SellerTitle"] = _sellerQueries.GetSellerTitle(Id);
            return View(ExpiredDiscounts);
        }
    }
}