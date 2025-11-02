
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Application.Contract.SiteServiceService.Command;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Site.Service
{
    [IgnoreAntiforgeryToken]
    public class ChangeActivationModel : PageModel
    {
        private readonly ISiteServiceCommandService _siteServiceService;

        public ChangeActivationModel(ISiteServiceCommandService siteServiceService)
        {
            _siteServiceService = siteServiceService;
        }

        public async Task<JsonResult> OnGet(int id)
        {
          
            if(id<1)
                return new JsonResult (new { success = false });

            var result= await _siteServiceService.ActivationChangeAsync(id);
            if (result.Success)
                return new JsonResult(new {success=true });

            return new JsonResult(new {success=false});
            
        }
    }
}
