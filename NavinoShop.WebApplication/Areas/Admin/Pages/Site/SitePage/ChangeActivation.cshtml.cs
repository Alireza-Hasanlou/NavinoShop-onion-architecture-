
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Application.Contract.SitePageService.Command;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Site.SitePage
{
    [IgnoreAntiforgeryToken]
    public class ChangeActivationModel : PageModel
    {
        private readonly ISitePageCommandService _sitePageService;

        public ChangeActivationModel(ISitePageCommandService sitePageService)
        {
            _sitePageService = sitePageService;
        }

        public async Task<JsonResult> OnGet(int id)
        {
          
            if(id<1)
                return new JsonResult (new { success = false });

            var result= await _sitePageService.ActivationChangeAsync(id);
            if (result.Success)
                return new JsonResult(new {success=true });

            return new JsonResult(new {success=false});
            
        }
    }
}
