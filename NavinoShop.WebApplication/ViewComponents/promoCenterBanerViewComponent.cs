using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Enums;
using Site.Application.Contract.BanerService.Query;

namespace NavinoShop.WebApplication.ViewComponents
{
    public class promoCenterBanerViewComponent:ViewComponent
    {
        private readonly IBanerQueryService _banerQueryService;

        public promoCenterBanerViewComponent(IBanerQueryService banerQueryService)
        {
            _banerQueryService = banerQueryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var SliderSidebaners = await _banerQueryService.GetForUi(3, BanerState.بنر_تبلیغاتی_سمت_راست_وسط_410x100);

            return View(SliderSidebaners);
        }
    }
}
