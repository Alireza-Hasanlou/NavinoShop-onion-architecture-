
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Application.Contract.MenuService.Query;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Site.Menu
{
    public class IndexModel : PageModel
    {
        private readonly IMenuQueryService _menurQueryService;

        public IndexModel(IMenuQueryService menuQueryService)
        {
            _menurQueryService = menuQueryService;
        }
        public MenuPageAdminQueryModel Menu { get; set; }
        public async Task OnGet(int parentId)
        {
            Menu = await _menurQueryService.GetForAdminAsync(parentId);
        }
    }
}
