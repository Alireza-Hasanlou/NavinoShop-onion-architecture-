using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Query.Contract.UI.UserPanel;
using Shared.Application.Auth;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class PanelController : Controller
    {
        private readonly IUserPanelQueryService _userPanelQueryService;
        private readonly IAuthService _authService;
        public PanelController(IUserPanelQueryService userPanelQueryService, IAuthService authService)
        {
            _userPanelQueryService = userPanelQueryService;
            _authService = authService;
        }

        [Route("/UserPanel/Profile")]
        public async Task<IActionResult> Profile()
        {
            var userId = _authService.GetLoginUserId();
            var user= await _userPanelQueryService.GetUserInfoForPanel(userId);
            return View(user);
        }
    }
}
