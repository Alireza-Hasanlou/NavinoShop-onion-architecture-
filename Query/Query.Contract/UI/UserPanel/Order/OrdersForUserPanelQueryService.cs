using Shared.Domain.Enums;

namespace Query.Contract.UI.UserPanel.Order
{
    public record OrdersForUserPanelQueryService(int orderId, string OrderDate, int paymentPrice , OrderStatus status);
}