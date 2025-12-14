using Microsoft.AspNetCore.Mvc;
using Site.Application.Contract.SliderService.Query;

namespace NavinoShop.WebApplication.Views.Shared.Components
{
    public class SliderViewComponent:ViewComponent
    {
        private readonly ISliderQueryservice _sliderQueryService;

        public SliderViewComponent(ISliderQueryservice sliderQueryService)
        {
            _sliderQueryService = sliderQueryService;
        }
        
        public async Task <IViewComponentResult> InvokeAsync()
        {
            var slider = await _sliderQueryService.GetAllForUi();

            return View(slider);
        }
    }
}
