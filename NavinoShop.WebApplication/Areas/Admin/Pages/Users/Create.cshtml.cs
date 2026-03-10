using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Domain.Enums;
using Users.Application.Contract.UserService.Command;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Users
{
    [IgnoreAntiforgeryToken]
    public class CreateModel : PageModel
    {
        private readonly IUserCommandService _userCommandService;

        public CreateModel(IUserCommandService userCommandService)
        {
            _userCommandService = userCommandService;
        }
        public IActionResult OnGet()
        {
            return Partial("_CreateUserPartialView");
        }
        public async Task<JsonResult> OnPost(string Mobile, Gender Gender, string Password, string? FullName, string? Email)
        {
            if (string.IsNullOrWhiteSpace(Mobile))
                return new JsonResult(new { success = false, message = "شماره موبایل نمیتواند خالی باشد" });
            var res = await _userCommandService.CreateAsync(new CreateUserCommand
            {
                Mobile = Mobile,
                FullName = FullName,
                Email = Email,
                UserGender = Gender
            });
            if (res.Success)
                return new JsonResult(new JsonResult(new { success = true, message = "کاربر با موفقیت اضافه شد" }));
            return new JsonResult(new { success = false, message = res.Message });
        }
    }
}
