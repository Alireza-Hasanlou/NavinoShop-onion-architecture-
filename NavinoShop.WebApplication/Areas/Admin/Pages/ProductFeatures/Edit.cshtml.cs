using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Contract.ProductFeature.Command;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.ProductFeatures
{
    [IgnoreAntiforgeryToken]
    public class EditModel : PageModel
    {
        private readonly IProductFeatureCommands _productFeatureCommands;

        public EditModel(IProductFeatureCommands productFeatureCommands)
        {
            _productFeatureCommands = productFeatureCommands;
        }

        public async Task<IActionResult> OnGet(int featureId)
        {
            var feature = await _productFeatureCommands.GetForEditAsync(featureId);
            return Partial("_EditProductFeaturePartial", feature);
        }
        public async Task<JsonResult> OnPost(EditProductFeatureCommandModel model)
        {
            var res = await _productFeatureCommands.EditAsync(model);
            return new JsonResult(new {success=res.Success,message=res.Message});   
        }
    }
}
