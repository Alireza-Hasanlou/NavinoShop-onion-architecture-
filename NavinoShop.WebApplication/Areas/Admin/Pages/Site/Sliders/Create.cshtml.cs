using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Application.Contract.SliderService.Command;
namespace NavinoShop.WebApplication.Areas.Admin.Pages.Site.Sliders
{

    public class CreateModel : PageModel
    {
        private readonly ISliderCommandService _sliderCommandService;

        public CreateModel(ISliderCommandService sliderCommandService)
        {
            _sliderCommandService = sliderCommandService;
        }

        [BindProperty]
        public CreateSliderCommandModel CreateSlider { get; set; }
        public void OnGet()
        {
            Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                Page();

            var result = await _sliderCommandService.CreateAsync(CreateSlider);
            if (result.Success)
            {
                TempData["Success"] = "افرودن بنر جدید با موفقیت انجام شد";
                return RedirectToPage("Index");
            }
            ModelState.AddModelError($"RoleTitle.{result.ModelName}", result.Message);
            return Page();
        }


    }
}
