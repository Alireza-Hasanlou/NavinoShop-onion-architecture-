using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Users.Application.Contract.UserService.Command;
using Users.Application.Contract.UserService.Query;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly IUserQueryService _userQueryService;
        private readonly IUserCommandService _userCommandService;

        public EditModel(IUserQueryService userQueryService, IUserCommandService userCommandService)
        {
            _userQueryService = userQueryService;
            _userCommandService = userCommandService;
        }
        [BindProperty]
        public EditUserByAdminDto EditUserModel { get; set; }
        public async Task<IActionResult> OnGet(int Id)
        {
            if (Id < 0)
                return RedirectToPage("Index");
            EditUserModel = await _userQueryService.GetForEditByAdminAsync(Id);
            if (EditUserModel == null)
            {
                ViewData["success"] = "کاربری با آیدی ارسال شده یافت نشد";
                return RedirectToPage("Index");
            }
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();
            var res = await _userCommandService.EditByAdminAsync(EditUserModel);
            if (res.Success)
            {
                ViewData["success"] = "کاربر با موفقیت ویرایش شد";
                return RedirectToPage("Index");

            }
            ViewData["success"] = "خطا در انجام عملیات";
            return RedirectToPage("Index");
        }
    }
}
