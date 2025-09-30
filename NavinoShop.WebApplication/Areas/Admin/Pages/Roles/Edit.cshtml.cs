using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Users.Application.Contract.RoleService.Command;
using Users.Application.Contract.RoleService.Query;
using Utility.Shared.Application;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Roles
{
    [IgnoreAntiforgeryToken]
    public class EditModel : PageModel
    {
        public readonly IRoleQueryService _roleQueryService;
        private readonly IRoleCommandService _roleCommandService;

        public EditModel(IRoleQueryService roleQueryService, IRoleCommandService roleCommandService)
        {
            _roleQueryService = roleQueryService;
            _roleCommandService = roleCommandService;
        }

        [BindProperty]
        public EditRoleQueryModel Role { get; set; }
        public List<PermissionQueryModel> Permissions { get; set; }
        public async Task<IActionResult> OnGet(int RoleId)
        {
            Permissions = await _roleQueryService.GetAllPermission();
            Role = await _roleQueryService.GetForEdit(RoleId);
            return Page();
        }

        public async Task<IActionResult> OnPost(List<int> permissions)
        {
            if (!ModelState.IsValid)
            {
                Permissions = await _roleQueryService.GetAllPermission();
                return Page();

            }
            if (permissions.Count < 1)
            {
                Permissions = await _roleQueryService.GetAllPermission();
                ModelState.AddModelError(nameof(Role), ValidationMessages.ChoosePermission);
                return Page();


            }


            var result = await _roleCommandService.EditAsync(new EditRoleCommand { Id = Role.Id, Title = Role.Title }, permissions);
            if (result.Success)
            {
                TempData["success"] = "عملیات ویرایش با موفقیت انجام شد";
                return RedirectToPage("Index");
            }


            Permissions = await _roleQueryService.GetAllPermission();
            ModelState.AddModelError($"Role.{result.ModelName}", result.Message);
            return Page();
        }
    }
}
