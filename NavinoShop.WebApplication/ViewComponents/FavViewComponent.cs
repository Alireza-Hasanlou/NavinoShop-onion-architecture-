using Microsoft.AspNetCore.Mvc;
using Site.Application.Contract.SiteSettingService.Query;

namespace NavinoShop.WebApplication.ViewComponents
{
    public class FavViewComponent : ViewComponent
    {
        private readonly ISiteSettingQueryService _siteSettingQueryService;

        public FavViewComponent(ISiteSettingQueryService siteSettingQueryService)
        {
            _siteSettingQueryService = siteSettingQueryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var logo = await _siteSettingQueryService.GetFavIconForUi();

            return View(logo);
        }
    }
}
