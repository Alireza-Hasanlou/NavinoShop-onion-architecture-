
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Application.Contract.BanerService.Command;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Site.Baners
{
    [IgnoreAntiforgeryToken]
    public class ChangeActivationModel : PageModel
    {
        private readonly IBanerCommandService _banerService;

        public ChangeActivationModel(IBanerCommandService banerService)
        {
            _banerService = banerService;
        }
        public async Task<JsonResult> OnGet(int id)
        {
          
            if(id<1)
                return new JsonResult (new { success = false });

            var result= await _banerService.ActivationChangeAsync(id);
            if (result.Success)
                return new JsonResult(new {success=true });

            return new JsonResult(new {success=false});
            
        }
    }
}
