using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Users.Application.Contract.RoleService.Command;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Roles
{
    public class DeleteModel : PageModel
    {
        private readonly IRoleCommandService _roleCommandService;

        public DeleteModel(IRoleCommandService roleCommandService)
        {
            _roleCommandService = roleCommandService;
        }

        public int RoleId { get; set; }
        public async Task<JsonResult> OnGet(int id)
        {
            var result = await _roleCommandService.DeleteAsync(id);
            if (result.Success)
                return new JsonResult(new {success=true});

            return new JsonResult(new { success = false, errors = result.Message });

        }
    }
}
