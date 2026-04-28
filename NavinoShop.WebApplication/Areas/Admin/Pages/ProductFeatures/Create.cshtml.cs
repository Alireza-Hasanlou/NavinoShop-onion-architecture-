using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Contract.ProductFeature.Command;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.ProductFeatures
{
    [IgnoreAntiforgeryToken]
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IProductFeatureCommands _productFeatureCommands;

        public CreateModel(IProductFeatureCommands productFeatureCommands)
        {
            _productFeatureCommands = productFeatureCommands;
        }

        public IActionResult OnGet()
        {
            return Partial("_CreateProductFeaturePartial");
        }
        public async Task<JsonResult> OnPost(CreateProductFeatureCommandModel model)
        {
            var res = await _productFeatureCommands.CreateAsync(model);
            return new JsonResult(new { success = res.Success, message = res.Message });
        }
    }
}
