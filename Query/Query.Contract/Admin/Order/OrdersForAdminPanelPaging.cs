using Shared;
using Shared.Domain.Enums;
namespace Query.Contract.Admin.Order
{
    public class OrdersForAdminPanelPaging : BasePaging
    {
        public string Filter { get; set; }
        public string OrderId { get; set; }
       
        public int refId { get; set; }
        public OrderStatus status { get; set; }
        public List<OrderForAdminPanelQueryModel> Orders { get; set; }

    }
}
