using Discount.Application.Contract.OrderDiscounts.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Users.Application.Contract.RoleService.Command;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Discounts
{
    public class DeleteModel : PageModel
    {
        private readonly IOrderDiscountsCommands _orderDiscountsCommands;

        public DeleteModel(IOrderDiscountsCommands orderDiscountsCommands)
        {
            _orderDiscountsCommands = orderDiscountsCommands;
        }
        public async Task<JsonResult> OnGet(int id)
        {


            var result = await _orderDiscountsCommands.DeleteAsync(id);

            return new JsonResult(new { success = result.Success, errors = result.Message });

        }
    }
}
