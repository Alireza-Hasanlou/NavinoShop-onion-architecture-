using Shared.Domain.Enums;

namespace Query.Contract.UI.UserPanel.Order
{
    public class OrderUserPanelViewModel
    {
        public int OrderId { get; set; }
        public OrderPayment OrderPayment { get; set; }
        public int? OrderAddressId { get; set; }
        public string? DiscountTitle { get; set; }
        public int DiscountId { get; set; }
        public int DiscountPercent { get; set; }
        public int Price { get; set; }
        public int PriceAfterOff { get; set; }
        public int PaymentPriceSeller { get; set; }
        public int PostPrice { get; set; }
        public int PaymentPrice { get; set; }
        public int DiscountPrice { get; set; }
        public List<OrderSellerUserPanelQueryModel> OrderSellers { get; set; } = new();
        public OrderAddressQueryModel Address { get; set; }= new();


    }
}