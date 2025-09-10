using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Users.Application.Contract.RoleService.Command;
using Users.Application.Contract.RoleService.Query;
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
        [BindProperty]
        public List<PermissionQueryModel> Permissions { get; set; }
        public async Task<IActionResult> OnGet(int RoleId)
        {
            Permissions = await _roleQueryService.GetAllPermission();
            Role = await _roleQueryService.GetForEdit(RoleId);
            return Page();
        }

        public async Task<JsonResult> OnPost(List<int> permissions)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(Environment.NewLine,
           ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return new JsonResult(new { success = false, errors });

            }
            if(permissions.Count < 1)
                return new JsonResult(new { success = false, errors = "لطفا حداقل  یک دسترسی برای تقش انتخاب کنید" });


            var result = await _roleCommandService.EditAsync(new EditRoleCommand { Id = Role.Id, Title = Role.Title }, permissions);
            if (result.Success)
                return new JsonResult(new { success = true });

           
            return new JsonResult(new { success = false, errors=result.Message });
        }
    }
}
