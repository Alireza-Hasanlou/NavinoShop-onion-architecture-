using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Query.Contract.Admin.Order;
using Shared.Domain.Enums;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Orders
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IOrderAdminQueryService _orderAdminQueryService;

        public IndexModel(IOrderAdminQueryService orderAdminQueryService)
        {
            _orderAdminQueryService = orderAdminQueryService;
        }

        public OrdersForAdminPanelPaging OrdersPageModel { get; set; }
        public async Task<IActionResult> OnGet(int orderId, int refId, int pageId, OrderStatus status, string filter = "")
        {

            OrdersPageModel = await _orderAdminQueryService.GetOrdersAsync(orderId, refId, pageId, status, filter);
            return Page();
        }

        public async Task<IActionResult> OnPost(int orderId, int refId, int pageId, OrderStatus status, string filter = "")
        {

            var orders = await _orderAdminQueryService.GetOrdersAsync(orderId, refId, pageId, status, filter);
            return Partial("_OrdersListPartial", orders);
        }


    }
}
