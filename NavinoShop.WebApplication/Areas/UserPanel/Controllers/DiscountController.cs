using Microsoft.AspNetCore.Mvc;

namespace NavinoShop.WebApplication.Areas.UserPanel.Controllers
{
    using Discount.Application.Contract.ProductDiscount.Command;
    using Discount.Domain.ProductDiscountAgg;
    using Microsoft.AspNetCore.Mvc;
    using Shop.Application.Contract.ProductSell.Command;
    using Shop.Application.Contract.ProductSell.Query;
    using System.Threading.Tasks;
    [IgnoreAntiforgeryToken]
    [Area("UserPanel")]

    public class DiscountController : Controller
    {
        private readonly IProductDiscountCommands _productDiscountCommands;
        private readonly IProductSellQueries _productSellQueries;

        public DiscountController(IProductDiscountCommands productDiscountCommands, IProductSellQueries productSellQueries)
        {
            _productDiscountCommands = productDiscountCommands;
            _productSellQueries = productSellQueries;
        }

        [HttpGet]
        [Route("/Profile/[controller]/[action]/{ProductSellId}/{ProductId}")]
        public async Task<IActionResult> AddDiscount(int ProductSellId, int ProductId)
        {
            if (ProductId < 1 || ProductSellId < 1)
                return Json(new { suceess = false, message = "داده های نامعتبر" });
            var discount = await _productDiscountCommands.GetForUpsertAsync(ProductId, ProductSellId);
            if (discount == null)
                discount = new UpsertProductDiscountCommandModel()
                {
                    ProductId = ProductId,
                    ProductSellId = ProductSellId
                };

            return PartialView("_AddDiscountPartial", discount);
        }


        [HttpPost]
        [Route("/Profile/[controller]/[action]")]
        public async Task<IActionResult> AddDiscount(UpsertProductDiscountCommandModel command)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "داده های نامعتبر " });
            }
            if (command.ProductSellId > 0)
            {
                var haveAmount = await _productSellQueries.ProductSellHaveAmount(command.ProductSellId);
                if (!haveAmount)
                    return Json(new { success = false, message = "محصول مورد نظر در انبار موجود نیست " });


            }
            var result = await _productDiscountCommands.UpSertAsync(command);

            if (result.Success)
            {

                return Json(new { success = true, message = result.Message, data = result.Data });
            }

            return Json(new { success = false, message = result.Message });
        }



        [HttpPost]
        public async Task<IActionResult> DeleteDiscount(int id)
        {
            var result = await _productDiscountCommands.DeleteAsync(id);

            if (result.Success)
            {
                return Json(new { success = true, message = result.Message });
            }

            return Json(new { success = false, message = result.Message });
        }


    }
}
