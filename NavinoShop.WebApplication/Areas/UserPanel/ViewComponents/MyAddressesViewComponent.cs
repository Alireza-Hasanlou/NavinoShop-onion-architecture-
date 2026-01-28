using Microsoft.AspNetCore.Mvc;
using Query.Contract.UI.UserPanel;
using Shared.Application.Auth;
using Users.Application.Contract.UserAddressService.Query;

namespace NavinoShop.WebApplication.Areas.UserPanel.ViewComponents
{
    public class MyAddressesViewComponent : ViewComponent
    {
        private readonly IUserPanelQueryService _panelQueryService;
        private readonly IAuthService _authService;

        public MyAddressesViewComponent(IUserPanelQueryService panelQueryService, IAuthService authService)
        {
            _panelQueryService = panelQueryService;
            _authService = authService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = _authService.GetLoginUserId();
            var address = await _panelQueryService.GetUserAddressesAsync(userId);
            return View(address);
        }
    }
}
