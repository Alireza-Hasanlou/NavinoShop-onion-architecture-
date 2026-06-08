using Discount.Application.Contract.OrderDiscounts.Command;
using Discount.Application.Contract.OrderDiscounts.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Domain.Enums;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Discounts
{
    public class IndexModel : PageModel
    {
        private readonly IOrderDiscountsQueries _orderDiscountsQueries;

        public IndexModel(IOrderDiscountsQueries orderDiscountsQueries)
        {
            _orderDiscountsQueries = orderDiscountsQueries;
        }
        public List<OrderDiscountsQueryModel> Discounts { get; set; }
        public async Task OnGet()
        {
            Discounts = await _orderDiscountsQueries.GeAllAsync(0, OrderDiscountType.Order);
        }
    }
}
