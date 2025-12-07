using Microsoft.AspNetCore.Mvc;
using Site.Application.Contract.SiteSettingService.Query;

namespace NavinoShop.WebApplication.ViewComponents
{
    public class LogoHeaderViewComponent : ViewComponent
    {
        private readonly ISiteSettingQueryService _siteSettingQueryService;

        public LogoHeaderViewComponent(ISiteSettingQueryService siteSettingQueryService)
        {
            _siteSettingQueryService = siteSettingQueryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var logo = await _siteSettingQueryService.GetLogoForUi();

            return View(logo);
        }
    }
}
