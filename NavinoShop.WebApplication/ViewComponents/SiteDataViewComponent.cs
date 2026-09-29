using Microsoft.AspNetCore.Mvc;
using Query.Contract.Admin.Data;

namespace NavinoShop.WebApplication.ViewComponents
{
    public class SiteDataViewComponent : ViewComponent
    {
        private readonly IAdminDataQueryService _adminDataQueryService;

        public SiteDataViewComponent(IAdminDataQueryService adminDataQueryService)
        {
            _adminDataQueryService = adminDataQueryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model  = await _adminDataQueryService.GetSiteDataAsync();
            return View(model);
        }
    }
 
}
