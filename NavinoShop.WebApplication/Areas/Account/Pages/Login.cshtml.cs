using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Users.Application.Contract.UserService.Command;
using Utility.Shared.Application;

namespace NavinoShop.WebApplication.Areas.Account
{
    public class LoginModel : PageModel
    {

        private readonly IUserCommandService _userService;

        [BindProperty]
        public LoginUserCommand loginModel { get; set; }
        public LoginModel(IUserCommandService userService)
        {
            _userService = userService;
        }

        public async Task OnGet(string ReturnUrl = "/")
        {
            loginModel = new LoginUserCommand() { ReturnUrl = ReturnUrl };

        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
               return Page();

            var result = await _userService.LoginAsync(loginModel);
            if (result.Success)
            {
                return Redirect(loginModel.ReturnUrl);
            }
            else
            {
                ModelState.AddModelError(nameof(loginModel), result.Message);
                return Page();

            }
        }

    
    }
}
