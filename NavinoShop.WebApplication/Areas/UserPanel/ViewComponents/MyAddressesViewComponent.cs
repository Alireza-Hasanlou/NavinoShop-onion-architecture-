using Microsoft.AspNetCore.Mvc;
using Query.Contract.UI.UserPanel;
using Query.Contract.UI.UserPanel.UserAddress;
using Shared.Application.Auth;
using Users.Application.Contract.UserAddressService.Query;

namespace NavinoShop.WebApplication.Areas.UserPanel.ViewComponents
{
    public class MyAddressesViewComponent : ViewComponent
    {
        private readonly IUserAddressUiQueryService _userAddressUiQueryService;
        private readonly IAuthService _authService;

        public MyAddressesViewComponent(IUserAddressUiQueryService userAddressUiQueryService, IAuthService authService)
        {
            _userAddressUiQueryService = userAddressUiQueryService;
            _authService = authService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = _authService.GetLoginUserId();
            var address = await _userAddressUiQueryService.GetUserAddressesAsync(userId);
            return View(address);
        }
    }
}
