using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostModule.Application.Contract.StateQuery;
using Query.Contract.UI.UserPanel;
using Query.Contract.UI.UserPanel.Order;
using Query.Contract.UI.UserPanel.Wallet;
using Shared.Application.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;
using Users.Application.Contract.UserAddressService.Command;
using Users.Application.Contract.UserService.Command;


namespace NavinoShop.WebApplication.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Route("/Profile/[action]")]
    [Authorize]
    public class PanelController : Controller
    {
        private readonly IUserPanelQueryService _userPanelQueryService;
        private readonly IAuthService _authService;
        private readonly IOrderUserPanelQueryService _orderUserPanelQueryService;
        private readonly IUserCommandService _userCommandService;
        private int _userId;


        public PanelController(IUserPanelQueryService userPanelQueryService, IAuthService authService,
            IUserCommandService userCommandService, IOrderUserPanelQueryService orderUserPanelQueryService)
        {
            _userPanelQueryService = userPanelQueryService;
            _authService = authService;
            _userCommandService = userCommandService;
            _orderUserPanelQueryService = orderUserPanelQueryService;

        }

        public async Task<IActionResult> PersonalInfo()
        {

            var userId = _authService.GetLoginUserId();
            var user = await _userPanelQueryService.GetUserInfoForPanel(userId);
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile(bool status = false)
        {
            //Edit User Status
            if (status)
            {
                ViewData["editProfileSuccess"] = "عملیات با موفقیت انجام شد";
            }
            var userId = _authService.GetLoginUserId();
            var res = await _userCommandService.GetForEditByUserAsync(userId);
            return View(res);
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile(EditUserByUserCommand command)
        {
            if (!ModelState.IsValid)
                return View(command);
            var userId = _authService.GetLoginUserId();
            var res = await _userCommandService.EditByUserAsync(command, userId);
            if (res.Success)
            {
                return RedirectToAction("EditProfile", new { status = true });
            }
            ModelState.AddModelError("Email", res.Message);
            return View(command);
        }
        public async Task<IActionResult> Orders()
        {
            _userId = _authService.GetLoginUserId();
            List<OrdersForUserPanelQueryService> Orders = await _orderUserPanelQueryService.GetOrdersAsync(_userId);
            return View(Orders);
        }
        public async Task <IActionResult> OrderDetails(int orderId)
        {
            _userId = _authService.GetLoginUserId();
            OrderUserPanelViewModel order = await _orderUserPanelQueryService.GetOrderDetailsAsync(_userId, orderId);
            if (order == null || order.OrderId == 0)
                return NotFound();
            return View(order);
        }
    }
}
