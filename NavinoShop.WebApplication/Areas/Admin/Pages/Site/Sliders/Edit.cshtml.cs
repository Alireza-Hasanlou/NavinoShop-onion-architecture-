using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Application.Contract.SliderService.Command;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Site.Sliders
{
    public class EditModel : PageModel
    {
        private readonly ISliderCommandService _sliderService;

        public EditModel(ISliderCommandService sliderService)
        {
            _sliderService = sliderService;
        }

        [BindProperty]
        public EditSliderCommandModel EditSlider { get; set; }
        public async Task OnGet(int sliderId)
        {
            EditSlider = await _sliderService.GetForEditAsync(sliderId);
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _sliderService.EditAsync(EditSlider);
            if (result.Success)
            {
                TempData["success"] = "اسلایدر با موفقیت ویرایش شد";
                return Redirect("/Admin/Site/sliders/Index");
            }
            ModelState.AddModelError($"Editslider.{result.ModelName}", result.Message);
            return Page();
        }
    }
}
