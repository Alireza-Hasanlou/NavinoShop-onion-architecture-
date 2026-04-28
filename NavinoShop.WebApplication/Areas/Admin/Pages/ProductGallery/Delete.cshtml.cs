using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NavinoShop.WebApplication.Utility;
using Shop.Application.Contract.ProductFeature.Command;
using Shop.Application.Contract.ProductGallery.Command;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.ProductGallery
{
    [IgnoreAntiforgeryToken]
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly IProductGalleryCommands _productGalleryCommands;

        public DeleteModel(IProductGalleryCommands productGalleryCommands)
        {
            _productGalleryCommands = productGalleryCommands;
        }

        public async Task<JsonResult> OnGet(int Id)
        {
            var res = await _productGalleryCommands.DeleteAsync(Id);
            if (res.Success)
                return new JsonResult(new { success = true, message = "تصویر با موفقیت حذف شد" });

            return new JsonResult(new { success = false, message = res.Message });
        }
    }
}
