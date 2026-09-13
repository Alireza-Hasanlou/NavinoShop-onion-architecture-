using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Query.Contract.Admin.Order;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Orders
{
    public class OrderDetailsModel : PageModel
    {
        private readonly IOrderAdminQueryService _orderAdminQueryService;


        public OrderDetailsModel(IOrderAdminQueryService orderAdminQueryService)
        {
            _orderAdminQueryService = orderAdminQueryService;
        }
        public OrderDetailsForAdminQueryModel OrderDetails { get; set; }
        public async Task<IActionResult> OnGet(int orderId)
        {
            OrderDetails = await _orderAdminQueryService.GetOrderDetailsForAdminAsync(orderId);
            return Page();  
        }
    }
}
