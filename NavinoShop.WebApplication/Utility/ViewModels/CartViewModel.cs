namespace NavinoShop.WebApplication.Utility.ViewModels
{
    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public int TotalItems { get; set; }
    }
}
