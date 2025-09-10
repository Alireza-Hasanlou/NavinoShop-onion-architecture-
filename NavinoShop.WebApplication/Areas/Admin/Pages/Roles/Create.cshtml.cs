using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NavinoShop.WebApplication.Utility.ViewModels;
using Users.Application.Contract.RoleService.Command;
using Users.Application.Contract.RoleService.Query;
using Utility.Shared.Application;
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
        [BindProperty]

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
                var errors = string.Join(Environment.NewLine,
             ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return new JsonResult(new { success = false, errors });
            }
            if (permissions.Count < 1)
                return new JsonResult(new { success = false, errors = ValidationMessages.ChoosePermission });

            var result = await _roleCommandService.CreateAsync(RoleTitle, permissions);
            if (result.Success)
                return new JsonResult(new { success = true });

            return new JsonResult(new { success = false, errors = result.Message });
        }


    }
}
