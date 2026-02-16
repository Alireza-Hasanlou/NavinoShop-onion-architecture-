using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Users.Application.Contract.UserService.Command;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Users
{
    public class ChangeActivationModel : PageModel
    {
        private readonly IUserCommandService _userCommandService;

        public ChangeActivationModel(IUserCommandService userCommandService)
        {
            _userCommandService = userCommandService;
        }

        public async Task<JsonResult> OnGet(int Id)
        {
            if (Id < 1)
                return new JsonResult(new { success = false, title = "آیدی نامعتبر!" });
            var res = await _userCommandService.ActivationChangeAsync(Id);
            if (res.Success)
                return new JsonResult(new { success = true, title = "عملیات با موفقیت انجام شد" });

            return new JsonResult(new { success = false, title = "خطا در انجام عملیات" });
        }
    }
}
