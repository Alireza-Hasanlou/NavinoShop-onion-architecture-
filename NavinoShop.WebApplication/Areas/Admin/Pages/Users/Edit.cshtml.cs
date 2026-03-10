using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Users.Application.Contract.RoleService.Query;
using Users.Application.Contract.UserService.Command;
using Users.Application.Contract.UserService.Query;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly IUserQueryService _userQueryService;
        private readonly IUserCommandService _userCommandService;
        private readonly IRoleQueryService _roleQueryService;

        public EditModel(IUserQueryService userQueryService, IUserCommandService userCommandService,
            IRoleQueryService roleQueryService)
        {
            _userQueryService = userQueryService;
            _userCommandService = userCommandService;
            _roleQueryService = roleQueryService;
        }

        [BindProperty]
        public EditUserByAdminDto EditUserModel { get; set; }
        public async Task<IActionResult> OnGet(int Id)
        {
            if (Id < 0)
                return RedirectToPage("Index");
            ViewData["Roles"] = await _roleQueryService.GetAllRoles();
            EditUserModel = await _userQueryService.GetForEditByAdminAsync(Id);

            if (EditUserModel == null)
            {
                ViewData["success"] = "کاربری با آیدی ارسال شده یافت نشد";
                return RedirectToPage("Index");
            }
            return Page();
        }
        public async Task<IActionResult> OnPost(List<int> roles)
        {
            EditUserModel.UserRoleIds = roles;
            if (!ModelState.IsValid)
            {
                ViewData["Roles"] = await _roleQueryService.GetAllRoles();
                return Page();
            }

            var res = await _userCommandService.EditByAdminAsync(EditUserModel, roles);
            if (res.Success)
            {
                TempData["success"] = "کاربر با موفقیت ویرایش شد";
                return RedirectToPage("Index");

            }
            ViewData["Roles"] = await _roleQueryService.GetAllRoles();
            ModelState.AddModelError($"EditUserModel.{res.ModelName}", res.Message);
            return Page();
        }
    }
}
