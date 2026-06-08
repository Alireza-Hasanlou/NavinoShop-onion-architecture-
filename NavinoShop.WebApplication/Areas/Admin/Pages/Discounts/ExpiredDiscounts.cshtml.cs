using Discount.Application.Contract.OrderDiscounts.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Domain.Enums;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Discounts
{
    public class ExpiredDiscountsModel : PageModel
    {
        private readonly IOrderDiscountsQueries _orderDiscountsQueries;

        public ExpiredDiscountsModel(IOrderDiscountsQueries orderDiscountsQueries)
        {
            _orderDiscountsQueries = orderDiscountsQueries;
        }
        public List<OrderDiscountsQueryModel> ExpiredDiscounts { get; set; }
        public async Task OnGet()
        {
            ExpiredDiscounts = await _orderDiscountsQueries.GeAllExpiredDiscountsAsync(0, OrderDiscountType.Order);
        }
    }
}
