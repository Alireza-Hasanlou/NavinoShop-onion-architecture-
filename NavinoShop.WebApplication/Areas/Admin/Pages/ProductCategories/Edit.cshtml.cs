using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Contract.ProductCategory.Commands;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.ProductCategories
{
    [IgnoreAntiforgeryToken]
    public class EditModel : PageModel
    {
        private readonly IProductCategoryCommands _productCategoryCommands;

        public EditModel(IProductCategoryCommands productCategoryCommands)
        {
            _productCategoryCommands = productCategoryCommands;
        }

        public EditProductCategoryCommandModel ProductCategory { get; set; }
        public async Task<IActionResult> OnGet(int Id)
        {
            ProductCategory = await _productCategoryCommands.GetForEditAsync(Id);
            return Partial("_EditProductCategoryPartial", ProductCategory);
        }
        public async Task<IActionResult> OnPost(EditProductCategoryCommandModel model)
        {
            var res = await _productCategoryCommands.EditAsync(new EditProductCategoryCommandModel
            {
                ParentId = model.ParentId,
                Title =model. Title,
                Slug = model.Slug,
                Id = model.Id
            });

            if (res.Success)
            {
                return new JsonResult(new { success = true, message = "دسته بندی جدید با موفقیت ویرایش شد " });
            }
            return new JsonResult(new { success = false, message = res.Message });
        }
    }
}
