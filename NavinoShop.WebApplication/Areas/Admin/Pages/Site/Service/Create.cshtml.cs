using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Application.Contract.SiteServiceService.Command;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Site.Service
{
    public class CreateModel : PageModel
    {
        private readonly ISiteServiceCommandService _SiteService;

        public CreateModel(ISiteServiceCommandService SiteService)
        {
             _SiteService =  SiteService;
        }
        [BindProperty]
        public CreateSiteServiceCommnadModel CreateSiteService { get; set; }
        public void OnGet() => Page();
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                Page();
        
            var result = await _SiteService.CreateAsync(CreateSiteService);
            if (result.Success)
            {
                TempData["Success"] = "افرودن تصویر جدید با موفقیت انجام شد";
                return RedirectToPage("Index");
            }
            ModelState.AddModelError($"CreateSiteService.{result.ModelName}", result.Message);
            return Page();
        }
    }
}
