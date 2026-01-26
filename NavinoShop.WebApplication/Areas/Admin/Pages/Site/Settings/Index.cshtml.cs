using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Application.Contract.SiteSettingService.Command;


namespace NavinoShop.WebApplication.Areas.Admin.Pages.Site.Settings
{
    public class IndexModel : PageModel
    {
        private readonly ISiteSettingService _siteSettingService;

        public IndexModel(ISiteSettingService siteSettingService)
        {
            _siteSettingService = siteSettingService;
        }
        [BindProperty]
        public UpsertSiteSetting UpsertSiteSetting { get; set; }
        public async Task<IActionResult> OnGet() 
        {
            UpsertSiteSetting = await _siteSettingService.GetForUpsert(); 
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            UpsertSiteSetting = await _siteSettingService.GetForUpsert();
            if (!ModelState.IsValid)
                return Page();
            var result = await _siteSettingService.Upsert(UpsertSiteSetting);
            if (result.Success)
            {
                TempData["Success"] = "تنظیمات اعمال شد";
                return  Page();

            }
            ModelState.AddModelError($"UpsertSiteSetting.{result.ModelName}", result.Message);
            return Page();


        }
    }
}
