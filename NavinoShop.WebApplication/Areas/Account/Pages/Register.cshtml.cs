using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Users.Application.Contract.UserService.Command;
using Utility.Shared.Application;

namespace NavinoShop.WebApplication.Areas.Account.Pages
{
    [IgnoreAntiforgeryToken]
    public partial class RegisterModel : PageModel
    {
        private readonly IUserCommandService _userService;
        [BindProperty]
        public RegisterUserCommand registerUserModel { get; set; }
        public RegisterModel(IUserCommandService userService)
        {
            _userService = userService;
        }

        public void OnGet()
        {

        }
        public async Task<JsonResult> OnPostSendCode()
        {
            if (!ModelState.IsValid)
                return new JsonResult(new { success = false });


            var result = await _userService.RegisterAsync(registerUserModel);
            if (result.Success)
            {
                return new JsonResult(new { success = true });
            }
            else
            {
                ModelState.AddModelError(nameof(registerUserModel), ValidationMessages.SystemErrorMessage);
                return new JsonResult(new { success = false });
            }


        }
        public IActionResult OnPostVerifyCode([FromBody] string dto)
        {

            var savedCode = HttpContext.Session.GetString("VerifyCode_" + dto);
            if (savedCode != null && savedCode == dto)
            {
                return new JsonResult(new { success = true });
            }

            return new JsonResult(new { success = false });
        }

    }
}
