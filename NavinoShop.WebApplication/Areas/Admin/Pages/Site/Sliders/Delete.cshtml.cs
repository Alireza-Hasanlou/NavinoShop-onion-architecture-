using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Application.Contract.BanerService.Command;
using Site.Application.Contract.SliderService.Command;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Site.Sliders
{
    public class DeleteModel : PageModel
    {
        private readonly ISliderCommandService _sliderCommandService;

        public DeleteModel(ISliderCommandService sliderCommandService)
        {
            _sliderCommandService = sliderCommandService;
        }

        public async Task<JsonResult> OnGet(int id)
        {
            var result = await _sliderCommandService.DeleteAsync(id);
            if (result.Success)
                return new JsonResult(new { success = true, title = "اسلایدر با موفقیت حذف شد" });

            return new JsonResult(new { success = false, errors = result.Message });

        }
    }
}
