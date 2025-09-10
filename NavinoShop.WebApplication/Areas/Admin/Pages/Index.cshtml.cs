
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NavinoShop.WebApplication.Utility;
using Users.Application.Contract.RoleService.Query;

namespace NavinoShop.WebApplication.Areas.Admin.Pages
{
    [PermissionChecker(1)]
    public class IndexModel : PageModel
    {
        private readonly IRoleQueryService _roleQueryService;

        public IndexModel(IRoleQueryService roleQueryService)
        {
            _roleQueryService = roleQueryService;
        }

        public async Task<IActionResult> OnGet()
        {

            return Page();
        }
    }
}
