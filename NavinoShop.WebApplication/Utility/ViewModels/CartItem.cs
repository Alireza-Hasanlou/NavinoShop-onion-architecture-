namespace NavinoShop.WebApplication.Utility.ViewModels
{
    public class CartItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public decimal Price { get; set; }
        public int quantity { get; set; }
        public decimal PriceAfterOff { get; set; }
        public string Seller { get; set; }
        public int Amount { get; set; }
    }
}
