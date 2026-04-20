using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Contract.Product.Command;
using Shop.Application.Contract.ProductCategory.Commands;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Products
{
    public class ChangeActivationModel : PageModel
    {
        private readonly IProductCommands _productCommands;

        public ChangeActivationModel(IProductCommands productCommands)
        {
            _productCommands = productCommands;
        }

        public async Task<IActionResult> OnGet(int id)
        {

            if (id < 1)
                return new JsonResult(new { success = false });

            var result = await _productCommands.ChangeActivation(id);
            if (result.Success)
                return new JsonResult(new { success = true });

            return new JsonResult(new { success = false });

        }
    }
}
