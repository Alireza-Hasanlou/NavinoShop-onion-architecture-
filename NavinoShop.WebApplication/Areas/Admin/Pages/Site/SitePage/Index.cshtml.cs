using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Application.Contract.SitePageService.Query;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Site.SitePage
{
    public class IndexModel : PageModel
    {
        private readonly ISitePageQueryService _sitePageQueryService;

        public IndexModel(ISitePageQueryService sitePageQueryService)
        {
            _sitePageQueryService = sitePageQueryService;
        }
        public List<SitePageAdminQueryModel> sitePages { get; set; }
        public async Task OnGet()
        {
            sitePages = await _sitePageQueryService.GetAllForAdmin();
        }
    }
}
