using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NavinoShop.WebApplication.Utility;
using Shop.Application.Contract.ProductFeature.Command;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.ProductFeatures
{
    [IgnoreAntiforgeryToken]
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly IProductFeatureCommands _productFeatureCommands;

        public DeleteModel(IProductFeatureCommands productFeatureCommands)
        {
            _productFeatureCommands = productFeatureCommands;
        }

        public async Task<JsonResult> OnGet(int Id)
        {
            var res = await _productFeatureCommands.DeleteAsync(Id);
            if (res.Success)
                return new JsonResult(new { success = true, message = "ویژگی محصول با موفقیت حذف شد" });

            return new JsonResult(new { success = false, message = res.Message });
        }
    }
}
