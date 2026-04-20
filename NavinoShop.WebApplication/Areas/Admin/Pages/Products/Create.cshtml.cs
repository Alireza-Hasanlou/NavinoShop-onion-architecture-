using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Contract.Product.Command;
using Shop.Application.Contract.ProductCategory.Query;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Products
{
    [IgnoreAntiforgeryToken]
    [Authorize]

    public class CreateModel : PageModel
    {
        private readonly IProductCommands _productCommands;
        private readonly IProductCategoryQueries _productCategoryQueries;

        public CreateModel(IProductCommands productCommands, IProductCategoryQueries productCategoryQueries)
        {
            _productCommands = productCommands;
            _productCategoryQueries = productCategoryQueries;
        }

        public async Task<IActionResult> OnGet()
        {
            var productCategories = await _productCategoryQueries.GetCategoriesForAddProduct();
            return Partial("_CreateProductPartial",productCategories);
        }

        public async Task<IActionResult> OnPost(CreateProductCommandModel model)
        {
            var res = await _productCommands.CreateAsync(model);
            if (res.Success)
                return new JsonResult(new { success = true });

            return new JsonResult(new { success = false, message = res.Message });
        }
    }
}
