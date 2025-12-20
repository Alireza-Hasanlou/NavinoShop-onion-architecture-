using Microsoft.AspNetCore.Mvc;
using Site.Application.Contract.SiteSettingService.Query;

namespace NavinoShop.WebApplication.ViewComponents
{
    public class FooterDescriptionViewComponent : ViewComponent
    {
        private readonly ISiteSettingQueryService _siteSettingQueryService;

        public FooterDescriptionViewComponent(ISiteSettingQueryService siteSettingQueryService)
        {
            _siteSettingQueryService = siteSettingQueryService;
        }
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var footerDescription = await _siteSettingQueryService.GetFooter();

            return View(footerDescription);
        }
    }

}
