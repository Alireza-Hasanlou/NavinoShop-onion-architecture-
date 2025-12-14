using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Enums;
using Site.Application.Contract.BanerService.Query;

namespace NavinoShop.WebApplication.ViewComponents
{
    public class promoCenterBanerLeftSideViewComponent : ViewComponent
    {
        private readonly IBanerQueryService _banerQueryService;

        public promoCenterBanerLeftSideViewComponent(IBanerQueryService banerQueryService)
        {
            _banerQueryService = banerQueryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var SliderSidebaners = await _banerQueryService.GetForUi(3, BanerState.بنر_تبلیغاتی_سمت_چپ_وسط_850x100);

            return View(SliderSidebaners);
        }
    }
}
