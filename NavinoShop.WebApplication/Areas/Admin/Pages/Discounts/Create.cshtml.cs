using Discount.Application.Contract.OrderDiscounts.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Domain.Enums;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Discounts
{
    [IgnoreAntiforgeryToken]
    [Area("admin")]
    public class CreateModel : PageModel
    {
        private readonly IOrderDiscountsCommands _orderDiscountsCommands;

        public CreateModel(IOrderDiscountsCommands orderDiscountsCommands)
        {
            _orderDiscountsCommands = orderDiscountsCommands;
        }
        public IActionResult OnGet()
        {
            return Partial("_CreateOrderDiscountPartial");
        }

        public async Task<IActionResult> OnPost(UpsertOrderDiscountCommandModel model)
        {
            if (!ModelState.IsValid)
                return Partial("_CreateOrderDiscountPartial", model);
            model.type = OrderDiscountType.Order;
            var result = await _orderDiscountsCommands.CreateOrderDiscountAsync(model);
            return new JsonResult(new { result.Success, result.Message });
        }
    }
}
