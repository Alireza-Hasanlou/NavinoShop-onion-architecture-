using AutoMapper;
using Discount.Application.Contract.OrderDiscounts.Command;
using Discount.Application.Contract.OrderDiscounts.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Query.Contract.UI.Cart;
using Query.Contract.UI.UserPanel.Order;
using Shared.Application;
using Shared.Application.Auth;
using Shop.Application.Contract.Order.Command;

namespace NavinoShop.WebApplication.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IOrderCommands _orderCommands;
        private readonly ICartUiQueryService _cartUiQueryService;
        private readonly IOrderDiscountsQueries _orderDiscountsQueries;
        private readonly IOrderDiscountsCommands _orderDiscountsCommands;
        private readonly IOrderUserPanelQueryService _orderUserPanelQuery;
        private readonly IMapper _mapper;
        private int _userId;

        public OrderController(IAuthService authService, IOrderCommands orderCommands,
            ICartUiQueryService cartUiQueryService, IOrderUserPanelQueryService orderUserPanelQuery, IMapper mapper
            , IOrderDiscountsQueries orderDiscountsQueries, IOrderDiscountsCommands orderDiscountsCommands)
        {

            _authService = authService;
            _orderCommands = orderCommands;
            _cartUiQueryService = cartUiQueryService;
            _orderUserPanelQuery = orderUserPanelQuery;
            _orderDiscountsQueries = orderDiscountsQueries;
            _orderDiscountsCommands = orderDiscountsCommands;
            _mapper = mapper;
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
            OperationResultOrderDiscount applyRes = await _orderCommands.ApplySellerDiscountAsync(_userId, SellerId, result.Id, result.Percent, result.Title);
            if (applyRes.Success)
                return new JsonResult(new { success = true, message = applyRes.Message });
            else
            {
                await _orderDiscountsCommands.MinusUseDiscountAsync(result.Id);
                return new JsonResult(new { success = false, message = applyRes.Message });
            }
         
        }
    }
}
