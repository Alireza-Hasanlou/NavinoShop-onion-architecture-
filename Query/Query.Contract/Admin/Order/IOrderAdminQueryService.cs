using Shared.Domain.Enums;
namespace Query.Contract.Admin.Order
{
    public interface IOrderAdminQueryService
    {
        Task<OrdersForAdminPanelPaging> GetOrdersAsync(int orderId, int refId, int pageId, OrderStatus status, string filter="");
        Task<OrderDetailsForAdminQueryModel> GetOrderDetailsForAdminAsync( int OrderId);
        Task<List<LatestOrdersForIndexPage>> GetLatestOrdersForIndexPageAsync();
    }
}
