using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Contract.ProductFeature.Command;
using Shop.Application.Contract.ProductGallery.Command;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.ProductGallery
{
    [IgnoreAntiforgeryToken]
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IProductGalleryCommands _productGalleryCommands;

        public CreateModel(IProductGalleryCommands productGalleryCommands)
        {
            _productGalleryCommands = productGalleryCommands;
        }

        public IActionResult OnGet()
        {
            return Partial("_CreateProductGalleryPartial");
        }
        public async Task<JsonResult> OnPost(CreateProductGalleryCommandModel model)
        {
            var res = await _productGalleryCommands.CreateAsync(model);
            return new JsonResult(new { success = res.Success, message = res.Message });
        }
    }
}
