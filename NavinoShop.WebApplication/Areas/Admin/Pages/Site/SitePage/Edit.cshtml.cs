using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Application.Contract.SitePageService.Command;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Site.SitePage
{
    public class EditModel : PageModel
    {
        private readonly ISitePageCommandService _sitePageService;

        public EditModel(ISitePageCommandService sitePageService)
        {
            _sitePageService = sitePageService;
        }

        [BindProperty]
        public EditSitePageCommandModel EditsitePage { get; set; }
        public async Task OnGet(int sitePageId)
        {
            EditsitePage = await _sitePageService.GetForEditAsync(sitePageId);
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _sitePageService.EditAsync(EditsitePage);
            if (result.Success)
            {
                TempData["success"] = "صفحه با موفقیت ویرایش شد";
                return Redirect("/Admin/sitePages/Index");
            }
            ModelState.AddModelError($"EditsitePage.{result.ModelName}", result.Message);
            return Page();
        }
    }
}
