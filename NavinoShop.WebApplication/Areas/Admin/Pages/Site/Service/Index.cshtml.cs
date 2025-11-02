using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Application.Contract.SiteServiceService.Query;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Site.Service
{
    public class IndexModel : PageModel
    {
        private readonly ISiteServiceQuery _siteServiceQuery;

        public IndexModel(ISiteServiceQuery siteServiceQuery)
        {
            _siteServiceQuery = siteServiceQuery;
        }
        public List<SiteServiceAdminQueryModel> SiteServices { get; set; }
        public async Task OnGet()
        {
            SiteServices = await _siteServiceQuery.GetAllForAdmin();
        }
    }
}
