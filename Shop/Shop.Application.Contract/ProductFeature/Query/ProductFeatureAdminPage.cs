namespace Shop.Application.Contract.ProductFeature.Query
{
    public class ProductFeatureAdminPage
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public List<ProductFeatureQueryModel> ProductFeatures { get; set; }
    }
}
