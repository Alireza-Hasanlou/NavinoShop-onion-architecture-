using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Application.Contract.ImageSiteService.Command;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Site.Images
{
    public class CreateModel : PageModel
    {
        private readonly IImageSiteCommandService _imageSiteService;

        public CreateModel(IImageSiteCommandService imageSiteService)
        {
            _imageSiteService = imageSiteService;
        }
        [BindProperty]
        public CreateImageSiteCommandModel CreateImage { get; set; }
        public void OnGet() => Page();
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                Page();
        
            var result = await _imageSiteService.CreateAsync(CreateImage);
            if (result.Success)
            {
                TempData["Success"] = "افرودن تصویر جدید با موفقیت انجام شد";
                return RedirectToPage("Index");
            }
            ModelState.AddModelError($"CreateImage.{result.ModelName}", result.Message);
            return Page();
        }
    }
}
