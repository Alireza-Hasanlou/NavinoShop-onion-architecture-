using Microsoft.AspNetCore.Mvc;
using Site.Application.Contract.SiteServiceService.Query;
using Site.Application.Contract.SiteSettingService.Query;

namespace NavinoShop.WebApplication.Areas.Blog.ViewComponents
{
    public class SocialsViewComponent : ViewComponent
    {
        private readonly ISiteSettingQueryService _siteSettingQueryService;

        public SocialsViewComponent(ISiteSettingQueryService siteSettingQueryService)
        {
            _siteSettingQueryService = siteSettingQueryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var socials = await _siteSettingQueryService.GetSocialForUi();
            return View(socials);
        }

    }
}