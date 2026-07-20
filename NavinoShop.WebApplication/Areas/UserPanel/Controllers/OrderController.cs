using AutoMapper;
using Discount.Application.Contract.OrderDiscounts.Command;
using Discount.Application.Contract.OrderDiscounts.Query;
using Discount.Application.Contract.ProductDiscount.Command;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Query.Contract.UI.Cart;
using Query.Contract.UI.UserPanel.Order;
using Query.Contract.UI.UserPanel.UserAddress;
using Shared.Application;
using Shared.Application.Auth;
using Shop.Application.Contract.Order.Command;
using Shop.Application.Contract.Order.Query;
using Shop.Domain.SellerAgg;
using Users.Application.Contract.UserAddressService.Command;
using Users.Application.Contract.UserAddressService.Query;

namespace NavinoShop.WebApplication.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class OrderController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IOrderCommands _orderCommands;
        private readonly ICartUiQueryService _cartUiQueryService;
        private readonly IOrderDiscountsQueries _orderDiscountsQueries;
        private readonly IUserAddressUiQueryService _userAddressUiQueryService;
        private readonly IOrderDiscountsCommands _orderDiscountsCommands;
        private readonly IOrderUserPanelQueryService _orderUserPanelQuery;
        private readonly IMapper _mapper;
        private int _userId;

        public OrderController(IAuthService authService, IOrderCommands orderCommands, ICartUiQueryService cartUiQueryService,
            IOrderDiscountsQueries orderDiscountsQueries, IOrderDiscountsCommands orderDiscountsCommands,
            IOrderUserPanelQueryService orderUserPanelQuery, IMapper mapper, IUserAddressUiQueryService userAddressUiQueryService)
        {
            _authService = authService;
            _orderCommands = orderCommands;
            _cartUiQueryService = cartUiQueryService;
            _orderDiscountsQueries = orderDiscountsQueries;
            _orderDiscountsCommands = orderDiscountsCommands;
            _orderUserPanelQuery = orderUserPanelQuery;
            _mapper = mapper;
            _userAddressUiQueryService = userAddressUiQueryService;
        }

        [Route("/checkout")]
        public async Task<IActionResult> Index()
        {

            _userId = _authService.GetLoginUserId();
            var cart = await _cartUiQueryService.GetAllAsync(_userId);
            var shopCartVm = _mapper.Map<List<ShopCartViewModel>>(cart);
            var UpsertRes = await _orderCommands.UpsertUserOrder(_userId, shopCartVm);
            if (!UpsertRes.Success)
            {
                ViewData["Error"] = UpsertRes.Message;
                return View(new OrderUserPanelViewModel());
            }
            OrderUserPanelViewModel order = await _orderUserPanelQuery.GetOrderAsync(_userId);

            return View(order);
        }
        [Route("/Order/ApplySellerDiscount")]
        public async Task<IActionResult> ApplySellerDiscount(int SellerId, string Code)
        {
            OperationResultOrderDiscount result = await _orderDiscountsQueries.GetOrderSellerDiscountAsync(SellerId, Code);
            if (!result.Success)
                return new JsonResult(new { success = false, message = result.Message });

            _userId = _authService.GetLoginUserId();
            PricesAfterApplyDiscountDto applyRes = await _orderCommands.ApplySellerDiscountAsync(_userId, SellerId, result.Id, result.Percent, result.Title);
            // قبل از ارسال به View

            if (applyRes.Success)
                return Json(applyRes);
            else
            {
                await _orderDiscountsCommands.MinusUseDiscountAsync(result.Id);
                return new JsonResult(new { success = false, message = applyRes.Message });
            }

        }
        [Route("/Order/RemoveSellerDiscount")]
        public async Task<IActionResult> RemoveSellerDiscount(int SellerId)
        {
            _userId = _authService.GetLoginUserId();
            PricesAfterApplyDiscountDto RemoveRes = await _orderCommands.RemoveSellerDiscountAsync(_userId, SellerId);
            if (!RemoveRes.Success)
                await _orderDiscountsCommands.MinusUseDiscountAsync(RemoveRes.DiscountId);
            return Json(RemoveRes);
        }
        [HttpPost]
        [Route("/Order/ApplyOrderDiscount")]
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
        [Route("/Order/RemoveOrderDiscount")]
        public async Task<IActionResult> RemoveOrderDiscount(int OrderId)
        {
            _userId = _authService.GetLoginUserId();
            PricesAfterApplyDiscountDto RemoveRes = await _orderCommands.RemoveOrderDiscountAsync(_userId, OrderId);
            if (!RemoveRes.Success)
                await _orderDiscountsCommands.MinusUseDiscountAsync(RemoveRes.DiscountId);
            return Json(RemoveRes);
        }

        [Route("/Order/GetUserAddresses")]
        public async Task<IActionResult> GetUserAddresses()
        {
            _userId = _authService.GetLoginUserId();

            var UserAddresses = await _userAddressUiQueryService.GetUserAddressesAsync(_userId);
            return Json(UserAddresses);

        }
        [Route("/Order/SelectOrderAddress")]
        public async Task<IActionResult> SelectOrderAddress()
        {
            return PartialView("_SelectOrderAddressPartial");
        }
    }
}
