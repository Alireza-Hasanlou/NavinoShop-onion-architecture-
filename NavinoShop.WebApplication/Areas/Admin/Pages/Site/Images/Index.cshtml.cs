using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Application.Contract.ImageSiteService.Query;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Site.Images
{
    public class IndexModel : PageModel
    {
        private readonly IImageSiteQueryService _imageSiteQueryService;

        public IndexModel(IImageSiteQueryService imageSiteQueryService)
        {
            _imageSiteQueryService = imageSiteQueryService;
        }
        [BindProperty]
        public ImageAdminPaging Images { get; set; }
        public void OnGet(int pageId=1, int take=10, string filter="")
        {
            Images = _imageSiteQueryService.GetAllForAdmin(pageId, take, filter);

        }
    }
}
