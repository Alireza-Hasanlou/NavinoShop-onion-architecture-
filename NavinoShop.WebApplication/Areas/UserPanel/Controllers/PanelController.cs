using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostModule.Application.Contract.StateQuery;
using Query.Contract.UI.UserPanel;
using Shared.Application.Auth;
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
        private readonly IUserCommandService _userCommandService;

        public PanelController(IUserPanelQueryService userPanelQueryService, IAuthService authService, IUserCommandService userCommandService)
        {
            _userPanelQueryService = userPanelQueryService;
            _authService = authService;
            _userCommandService = userCommandService;
        }

        public async Task<IActionResult> PersonalInfo()
        {
            var userId = _authService.GetLoginUserId();
            var user = await _userPanelQueryService.GetUserInfoForPanel(userId);
            return View(user);
        }
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var userId = _authService.GetLoginUserId();
            var res = await _userCommandService.GetForEditByUserAsync(userId);
            return View(res);
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile(EditUserByUserCommand command)
        {
            if (!ModelState.IsValid)
                return View();
            var userId = _authService.GetLoginUserId();
            var res = await _userCommandService.EditByUserAsync(command, userId);
            if (res.Success)
            {
                ViewData["editProfileSuccess"] = "عملیات با موفقیت انجام شد";
                return RedirectToAction("PersonalInfo");
            }
            ModelState.AddModelError("Email", res.Message);
            return View();
        }

    }
}
