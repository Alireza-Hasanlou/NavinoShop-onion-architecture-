using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Users.Application.Contract.RoleService.Query;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Roles
{
    public class IndexModel : PageModel
    {
        private readonly IRoleQueryService _roleService;

        public IndexModel(IRoleQueryService roleService)
        {
            _roleService = roleService;
        }

        public List<RoleQueryModel> Roles { get; set; }
        public async Task<IActionResult> OnGet()
        {
            Roles = await _roleService.GetAllRoles();
            return Page();

        }
    }
}
