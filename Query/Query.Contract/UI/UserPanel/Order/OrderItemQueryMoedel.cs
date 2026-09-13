namespace Query.Contract.UI.UserPanel.Order
{
    public class OrderItemQueryMoedel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ProductSellId { get; set; }
        public string ProductName { get; set; }
        public int SellerId { get; set; }
        public int Price { get; set; }
        public int PriceAfterOff { get; set; }
        public string ImageName { get; set; }
        public string ImageAlt { get; set; }
        public int Quantity { get; set; }

    }
}