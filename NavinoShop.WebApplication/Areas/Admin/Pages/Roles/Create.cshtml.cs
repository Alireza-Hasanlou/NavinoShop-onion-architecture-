using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NavinoShop.WebApplication.Utility.ViewModels;
using Shared.Application.Validations;
using Users.Application.Contract.RoleService.Command;
using Users.Application.Contract.RoleService.Query;
namespace NavinoShop.WebApplication.Areas.Admin.Pages.Roles
{
    [IgnoreAntiforgeryToken]
    public class CreateModel : PageModel
    {
        private readonly IRoleQueryService _roleQueryervice;
        private readonly IRoleCommandService _roleCommandService;

        public CreateModel(IRoleQueryService roleQueryervice, IRoleCommandService roleCommandService)
        {
            _roleQueryervice = roleQueryervice;
            _roleCommandService = roleCommandService;
        }

        [BindProperty]
        public CreateRoleCommand RoleTitle { get; set; }
        public List<PermissionQueryModel> Permissions { get; set; }
        public async Task<IActionResult> OnGet()
        {
            Permissions = await _roleQueryervice.GetAllPermission();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(List<int> permissions)
        {
            if (!ModelState.IsValid)
            {
                Permissions = await _roleQueryervice.GetAllPermission();
                return Page();
            }
            if (permissions.Count < 1)
            {
                Permissions = await _roleQueryervice.GetAllPermission();
                ModelState.AddModelError("RoleTitle.Title", ValidationMessages.ChoosePermission);
                return Page();

            }
            var result = await _roleCommandService.CreateAsync(RoleTitle, permissions);
            if (result.Success)
            {
                TempData["Success"] = "افرودن نقش جدید با موفقیت انجام شد";
                return RedirectToPage("Index");
            }

            Permissions = await _roleQueryervice.GetAllPermission();
            ModelState.AddModelError($"RoleTitle.{result.ModelName}", result.Message);
            return Page();
        }


    }
}
