namespace Query.Contract.UI.Products
{
    public class ProductUiQueryModel
    {

        public int Id { get; set; }
        public int ProductId { get; set; }
        public int SellerId { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Category { get; set; }
        public string CategorySlug { get; set; }
        public string SellerTitle { get; set; }
        public int Price { get; set; }
        public int PriceAfterOff { get; set; }
        public string ImageName { get; set; }
        public string ImageAlt { get; set; }
    }

}
