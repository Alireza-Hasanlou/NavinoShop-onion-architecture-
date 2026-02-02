using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Users.Application.Contract.UserService.Command;

namespace NavinoShop.WebApplication.Areas.Account.Pages
{
    [IgnoreAntiforgeryToken]
    public class LoginModel : PageModel
    {

        private readonly IUserCommandService _userService;

       
        public LoginModel(IUserCommandService userService)
        {
            _userService = userService;
        }

        public async Task<JsonResult> OnPost(string mobile, string password)
        {
            if (string.IsNullOrWhiteSpace(mobile) || string.IsNullOrWhiteSpace(password))
                return new JsonResult(new { success = false, message = "نام کاربری یا رمز عبور صحیح نیست" });

            var result = await _userService.LoginAsync(new LoginUserCommand { Mobile=mobile,Password=password});
            if (result.Success)
            {
                return new JsonResult(new { success = true });
            }
            else
            {
                return new JsonResult(new { success = false, message = result.Message });

            }
        }


    }
}
