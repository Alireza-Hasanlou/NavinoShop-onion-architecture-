namespace Query.Contract.UI.UserPanel.Order
{
    public class OrderSellerUserPanelQueryModel
    {
        public int Id { get; set; }
        public int SellerId { get; set; }
        public int SellerCityId { get; set; }
        public string SellerName { get; set; }
        public string? ImageName { get; set; }
        public int DiscountPercent { get; set; }
        public int DiscountId { get; set; }
        public string DiscountTitle { get; set; }
        public int PostPrice { get; set; }
        public string PostTitle { get; set; }
        public int Price { get; set; }
        public int PriceAfterOff { get; set; }
        public int PaymentPrice { get; set; }
        public int DiscountPrice { get; set; }
        public List<OrderItemQueryMoedel> Items { get; set; }

    }
}