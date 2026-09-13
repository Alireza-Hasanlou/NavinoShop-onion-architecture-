using Shared.Domain.Enums;

namespace Query.Contract.UI.UserPanel.Seller
{
    public class SellersOrderQueryModel
    {
        public int OrderId { get; set; }
        public int  CustomerId { get; set; }
        public string PayDate { get; set; }
        public OrderSellerStatus status { get; set; }
        public string CustomerName { get; set; }
        public int PaymentPrice { get; set; }

    }
}
