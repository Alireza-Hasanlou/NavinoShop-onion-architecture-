using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Contract.ProductCategory.Commands;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.ProductCategories
{
    public class ChangeActivationModel : PageModel
    {
        private readonly IProductCategoryCommands _productCategoryCommands;

        public ChangeActivationModel(IProductCategoryCommands productCategoryCommands)
        {
            _productCategoryCommands = productCategoryCommands;
        }

        public async Task<IActionResult>  OnGet(int id)
        {

            if (id < 1)
                return new JsonResult(new { success = false });

            var result = await _productCategoryCommands.ChangeActivation(id);
            if (result.Success)
                return new JsonResult(new { success = true });

            return new JsonResult(new { success = false });

        }
    }
}
