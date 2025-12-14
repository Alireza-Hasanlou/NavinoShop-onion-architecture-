using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Enums;
using Site.Application.Contract.BanerService.Query;
using Site.Application.Contract.SiteSettingService.Query;

namespace NavinoShop.WebApplication.ViewComponents
{
    public class SliderSideBanersViewComponent : ViewComponent
    {
        private readonly IBanerQueryService _banerQueryService;

        public SliderSideBanersViewComponent(IBanerQueryService banerQueryService)
        {
            _banerQueryService = banerQueryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var SliderSidebaners = await _banerQueryService.GetForUi(3, BanerState.بنر_سه_تایی_کنار_اسلایدر_400x163);

            return View(SliderSidebaners);  
        }
    }
}
