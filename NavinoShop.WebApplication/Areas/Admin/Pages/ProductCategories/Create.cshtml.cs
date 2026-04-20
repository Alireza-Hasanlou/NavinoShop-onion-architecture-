using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Contract.ProductCategory.Commands;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.ProductCategories
{
    [IgnoreAntiforgeryToken]
    public class CreateModel : PageModel
    {
        private readonly IProductCategoryCommands _productCategoryCommands;

        public CreateModel(IProductCategoryCommands productCategoryCommands)
        {
            _productCategoryCommands = productCategoryCommands;
        }

        public IActionResult OnGet(int Id)
        {
            return Partial("_CreateProductCategoryPartial",Id);
        }

        public async Task<IActionResult> Onpost(int ParentId, string ProductCategoryTitle, string Slug)
        {
            var res = await _productCategoryCommands.CreateAsync(new CreateProductCategoryCommandModel
            {
                ParentId = ParentId,
                Title = ProductCategoryTitle,
                Slug = Slug
            });

            if (res.Success)
            {
                return new JsonResult(new { success = true, message = "دسته بندی جدید با موفقیت اضافه شد " });
            }
            return new JsonResult(new { success = false, message = res.Message });
        }
    }
}
