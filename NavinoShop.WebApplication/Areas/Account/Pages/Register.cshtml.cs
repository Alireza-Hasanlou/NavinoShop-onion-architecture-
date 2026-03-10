using Financial.Application.Contract.WalletService.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Application.Validations;
using Users.Application.Contract.UserService.Command;

namespace NavinoShop.WebApplication.Areas.Account.Pages
{
    [IgnoreAntiforgeryToken]
    public class RegisterModel : PageModel
    {
        private readonly IUserCommandService _userService;
     

        public RegisterModel(IUserCommandService userService)
        {
            _userService = userService;
  
        }

        [BindProperty]
        public RegisterUserCommand registerUserModel { get; set; }
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
