using Microsoft.AspNetCore.Mvc;
using Shared.Application.Auth;
using Users.Application.Contract.UserService.Query;

namespace NavinoShop.WebApplication.ViewComponents
{
    public class HeaderDropDownMenuViewComponent : ViewComponent
    {
        private readonly IUserQueryService _userQueryService;
        private readonly IAuthService _authService;

        public HeaderDropDownMenuViewComponent(IUserQueryService userQueryService, IAuthService authService)
        {
            _userQueryService = userQueryService;
            _authService = authService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = _authService.GetLoginUserId();
            var userinfo = await _userQueryService.GetUserForHeader(userId);
            return View(userinfo);
        }
    }
}
