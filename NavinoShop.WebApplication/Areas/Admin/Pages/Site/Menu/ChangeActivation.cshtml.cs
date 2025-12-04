
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Application.Contract.MenuService.Command;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Site.Menu
{
    [IgnoreAntiforgeryToken]
    public class ChangeActivationModel : PageModel
    {
        private readonly IMenuCommandService _menurService;

        public ChangeActivationModel(IMenuCommandService menurService)
        {
            _menurService = menurService;
        }

        public async Task<JsonResult> OnGet(int Id)
        {
          
            if(Id < 1)
                return new JsonResult (new { success = false });

            var result= await _menurService.ActivationChangeAsync(Id);
            if (result.Success)
                return new JsonResult(new {success=true });

            return new JsonResult(new {success=false});
            
        }
    }
}
