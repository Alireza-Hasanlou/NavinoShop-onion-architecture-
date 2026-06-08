using Discount.Application.Contract.ProductDiscount.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Domain.ProductSellAgg;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Products
{
    [Area("admin")]
    [IgnoreAntiforgeryToken]

    public class AddDiscountModel : PageModel
    {
        private readonly IProductDiscountCommands _productDiscountCommands;

        public AddDiscountModel(
            IProductDiscountCommands productDiscountCommands)
        {
            _productDiscountCommands = productDiscountCommands;
        }
        public async Task<IActionResult> OnGet(int ProductId)
        {
            if (ProductId < 1)
                return new JsonResult(new { suceess = false, message = "داده های نامعتبر" });
            var discount = await _productDiscountCommands.GetForUpsertAsync(ProductId, 0);
            if (discount == null)
                discount = new UpsertProductDiscountCommandModel()
                {
                    ProductId = ProductId,
                    ProductSellId = 0
                };

            return Partial("_AddProductDiscountPartial", discount);
        }

        public async Task<IActionResult> OnPost(UpsertProductDiscountCommandModel model)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "داده های نامعتبر " });
            }

            var result = await _productDiscountCommands.UpSertAsync(model);

            if (result.Success)
            {

                return new JsonResult(new { success = true, message = result.Message, data = result.Data });
            }

            return new JsonResult(new { success = false, message = result.Message });
        }

    }
}
