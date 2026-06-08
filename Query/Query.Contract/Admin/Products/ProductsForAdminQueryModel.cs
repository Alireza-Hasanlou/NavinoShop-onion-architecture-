namespace Query.Contract.Admin.Products
{
    public class ProductsForAdminQueryModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string ShortDescription { get; set; }
        public int Weight { get; set; }
        public string Slug { get; set; }
        public int DiscountPercent { get; set; }
        public bool Active { get; set; }
    }
}
