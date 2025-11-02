using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Application.Contract.SiteServiceService.Command;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Site.Service
{
    public class EditModel : PageModel
    {
        private readonly ISiteServiceCommandService _siteServiceService;

        public EditModel(ISiteServiceCommandService siteserviceService)
        {
            _siteServiceService = siteserviceService;
        }

        [BindProperty]
        public EditSiteServiceCommandModel EditsiteService { get; set; }
        public async Task OnGet(int siteServiceId)
        {
            EditsiteService = await _siteServiceService.GetForEditAsync(siteServiceId);
        }  
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _siteServiceService.EditAsync(EditsiteService);
            if (result.Success)
            {
                TempData["success"] = "سرویس با موفقیت ویرایش شد";
                return Redirect("/Admin/SiteService/Index");
            }
            ModelState.AddModelError($"EditsitePage.{result.ModelName}", result.Message);
            return Page();
        }
    }
}
