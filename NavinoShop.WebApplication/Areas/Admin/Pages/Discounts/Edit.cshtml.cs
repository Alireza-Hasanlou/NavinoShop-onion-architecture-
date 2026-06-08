using Discount.Application.Contract.OrderDiscounts.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shared.Domain.Enums;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Discounts
{
    [IgnoreAntiforgeryToken]
    [Area("admin")]
    public class EditModel : PageModel
    {

        private readonly IOrderDiscountsCommands _orderDiscountsCommands;

        public EditModel(IOrderDiscountsCommands orderDiscountsCommands)
        {
            _orderDiscountsCommands = orderDiscountsCommands;
        }
        public async Task<IActionResult> OnGet(int DiscountId)
        {
            if (DiscountId < 1)
                return new JsonResult(new { success = false, message = "شناسه نامعتبر" });
            var Discount = await _orderDiscountsCommands.GetForEditAsync(DiscountId);
            return Partial("_EditOrderDiscountPartial", Discount);
        }

        public async Task<IActionResult> OnPost(int DiscountId, UpsertOrderDiscountCommandModel model)
        {
            if (DiscountId != model.Id || DiscountId == 0 || model.Id == 0)
                return new JsonResult(new { success = false, message = "شناسه نامعتبر" });
            model.type = OrderDiscountType.Order;
            var result = await _orderDiscountsCommands.EditOrderDiscountAsync(model);
            return new JsonResult(new { result.Success, result.Message });
        }

    }
}
