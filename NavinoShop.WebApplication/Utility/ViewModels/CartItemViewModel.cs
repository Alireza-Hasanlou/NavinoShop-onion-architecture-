namespace NavinoShop.WebApplication.Utility.ViewModels
{
    public class CartItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public decimal Price { get; set; }
        public decimal? PriceAfterOff { get; set; }
        public string Seller { get; set; }
        public int Quantity { get; set; }
        public int Amount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
