using Shared;
using Shared.Domain.Enums;

namespace Query.Contract.UI.UserPanel.Seller
{
    public class SellersOrdersPaging:BasePaging
    {
        public int  SellerId { get; set; }
        public string Filter { get; set; }
        public int OrderId { get; set; }
        public int RefId { get; set; }
        public OrderSellerStatus orderSellerStatus { get; set; }
        public List<SellersOrderQueryModel> Orders { get; set; }
    }
}
