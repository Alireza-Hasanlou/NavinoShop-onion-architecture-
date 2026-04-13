using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Query.Contract.UI.UserPanel.Seller;
using Shared.Application.Auth;
using Shop.Application.Contract.Seller.Command;
using Shop.Application.Contract.Seller.Query;

namespace NavinoShop.WebApplication.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Route("/[controller]/[action]")]
    [Authorize]
    public class SellerController : Controller
    {
        private readonly ISellerCommands _sellerCommands;
        private readonly IAuthService _authService;
        private readonly ISellerUserPanelQueries _sellerUserPanelQueries;

        public SellerController(ISellerCommands sellerCommands, IAuthService authService, ISellerUserPanelQueries sellerUserPanelQueries)
        {
            _sellerCommands = sellerCommands;
            _authService = authService;
            _sellerUserPanelQueries = sellerUserPanelQueries;
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
    }
}
