namespace Shop.Application.Contract.Product.Query
{
    public class ProductsForSellerQueryModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string ShortDescription { get; set; }
        public int Weight { get; set; }
        public string Slug { get; set; }
        public bool Active { get; set; }
        public int Price { get; set; }
        public int SoldCount { get; set; }
    }
}
