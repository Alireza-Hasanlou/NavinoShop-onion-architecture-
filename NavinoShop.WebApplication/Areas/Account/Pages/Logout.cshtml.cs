using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Users.Application.Contract.UserService.Command;

namespace NavinoShop.WebApplication.Areas.Account.Pages
{
    public class LogoutModel : PageModel
    {
       private readonly IUserCommandService _userService;

        public LogoutModel(IUserCommandService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> OnGet()
        {
            await _userService.LogoutAsync();
            return Redirect("/Account/Login");
        }
    }
}
