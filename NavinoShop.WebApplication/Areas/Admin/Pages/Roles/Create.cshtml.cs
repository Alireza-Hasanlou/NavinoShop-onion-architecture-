using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NavinoShop.WebApplication.Utility.ViewModels;
using Users.Application.Contract.RoleService.Command;
using Users.Application.Contract.RoleService.Query;
namespace NavinoShop.WebApplication.Areas.Admin.Pages.Roles
{
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

        public async Task<IActionResult> OnPost( List<int> permissions)
        {
            if (!ModelState.IsValid)
            {
                Permissions = await _roleQueryervice.GetAllPermission();
                return Page();
            }
               

           var result= await _roleCommandService.CreateAsync(RoleTitle, permissions);
            if (result.Success)
            {
                return RedirectToPage("Index");
            }
            Permissions = await _roleQueryervice.GetAllPermission();
            ModelState.AddModelError(result.ModelName, result.Message);
            return Page();
        }
    }
}
