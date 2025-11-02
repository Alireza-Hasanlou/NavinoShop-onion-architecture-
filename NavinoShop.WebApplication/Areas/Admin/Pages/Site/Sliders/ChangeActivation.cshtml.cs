
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Application.Contract.SliderService.Command;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Site.Sliders
{
    [IgnoreAntiforgeryToken]
    public class ChangeActivationModel : PageModel
    {
        private readonly ISliderCommandService _sliderService;

        public ChangeActivationModel(ISliderCommandService sliderService)
        {
            _sliderService = sliderService;
        }

        public async Task<JsonResult> OnGet(int id)
        {
          
            if(id<1)
                return new JsonResult (new { success = false });

            var result= await _sliderService.ActivationChangeAsync(id);
            if (result.Success)
                return new JsonResult(new {success=true });

            return new JsonResult(new {success=false});
            
        }
    }
}
