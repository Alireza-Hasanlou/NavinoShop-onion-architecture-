namespace Query.Contract.UI.UserPanel.Seller
{
    public class ProductsForSellerQueryModel
    {
        public int ProductId { get; set; }
        public int ProductSellId { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string ShortDescription { get; set; }
        public int Weight { get; set; }
        public int Count { get; set; }
        public bool Active { get; set; }
        public int AdminDiscountPercent { get; set; }
        public int SellerDiscountPercent { get; set; }
        public int Price { get; set; }
        public int SoldCount { get; set; }
    }
}
