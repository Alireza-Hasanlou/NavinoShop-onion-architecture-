using Shared.Domain.Enums;
namespace Query.Contract.Admin.Order
{
    public record OrderForAdminPanelQueryModel(int orderId, int CustomerId, string customerName, int refId, string OrderDate, int paymentPrice, OrderStatus status);

}
