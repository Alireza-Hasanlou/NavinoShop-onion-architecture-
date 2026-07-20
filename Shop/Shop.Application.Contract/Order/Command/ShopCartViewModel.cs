namespace Shop.Application.Contract.Order.Command
{
    public class ShopCartViewModel
    {
        public int Id { get; set; }
        public int ProductSellId { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public int Price { get; set; }
        public int PriceAfterOff { get; set; }
        public string Seller { get; set; }
        public int Quantity { get; set; }
        public int Amount { get; set; }

    }
}