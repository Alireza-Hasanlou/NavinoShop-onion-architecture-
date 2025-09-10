using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Users.Application.Contract.RoleService.Query;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Roles
{
    public class UsersInRoleModel : PageModel
    {
        private readonly IRoleQueryService _roleQueryService;

        public UsersInRoleModel(IRoleQueryService roleQueryService)
        {
            _roleQueryService = roleQueryService;
        }

        public async Task<JsonResult> OnGet(int id)
        {
            var usersInrole = await _roleQueryService.GetUsersInRole(id);
            return new JsonResult(usersInrole);
        }
    }
}
