using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Application.Contract.SitePageService.Command;
namespace NavinoShop.WebApplication.Areas.Admin.Pages.Site.SitePage
{

    public class CreateModel : PageModel
    {
        private readonly ISitePageCommandService _sitePageCommandService;

        public CreateModel(ISitePageCommandService sitePageCommandService)
        {
            _sitePageCommandService = sitePageCommandService;
        }

        [BindProperty]
        public CreateSitePageCommnadModel CreatesitePage { get; set; }
        public void OnGet()=> Page();
 
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                Page();

            var result = await _sitePageCommandService.CreateAsync(CreatesitePage);
            if (result.Success)
            {
                TempData["Success"] = "افرودن صفحه جدید با موفقیت انجام شد";
                return RedirectToPage("Index");
            }
            ModelState.AddModelError($"CreatesitePage.{result.ModelName}", result.Message);
            return Page();
        }


    }
}
