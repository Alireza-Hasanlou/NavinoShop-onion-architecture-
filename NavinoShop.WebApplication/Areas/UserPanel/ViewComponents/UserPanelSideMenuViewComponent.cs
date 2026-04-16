using Microsoft.AspNetCore.Mvc;
using Query.Contract.UI.UserPanel;

namespace NavinoShop.WebApplication.Areas.UserPanel.ViewComponents
{
    public class UserPanelSideMenuViewComponent:ViewComponent
    {
        private readonly IUserPanelQueryService _userPanelQueryService;

        public UserPanelSideMenuViewComponent(IUserPanelQueryService userPanelQueryService)
        {
            _userPanelQueryService = userPanelQueryService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int userId)
        {
            var memnu = await _userPanelQueryService.GetUserSidePanelMenu(userId);
            return View(memnu);
        }
    }
}
