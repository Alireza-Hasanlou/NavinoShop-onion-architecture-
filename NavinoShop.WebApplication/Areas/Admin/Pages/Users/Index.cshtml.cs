using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Users.Application.Contract.UserService.Query;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly IUserQueryService _userQueryService;

        public IndexModel(IUserQueryService userQueryService)
        {
            _userQueryService = userQueryService;
        }
        [BindProperty]
        public AdminUserPaging Users { get; set; }
        public async Task<IActionResult> OnGet(string filter = "", int pageId = 1, int take = 10)
        {
            Users = await _userQueryService.GetUsersForAdminAsync(pageId, take, filter);
            return Page();
        }
    }
}
