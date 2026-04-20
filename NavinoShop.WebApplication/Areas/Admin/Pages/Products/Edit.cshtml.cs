using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Application;
using Shop.Application.Contract.Product.Command;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Products
{
    [IgnoreAntiforgeryToken]
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IProductCommands _productCommands;

        public EditModel(IProductCommands productCommands)
        {
            _productCommands = productCommands;
        }


        public async Task<IActionResult> OnGet(int Id)
        {
            if (Id < 1)
                return NotFound();
            var product = await _productCommands.GetForEditAsync(Id);
            return Partial("_EditProductPartial", product);
        }
        public async Task<JsonResult> OnPost(EditProductCommandModel model)
        {
            var res = await _productCommands.EditAsync(model);
            if (res.Success)
                return new JsonResult(new { success = true });
            return new JsonResult(new { success = false, message = res.Message });
        }
    }
}
