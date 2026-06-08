using Query.Contract.UI.Seo;
using Shared.Ui;

namespace Query.Contract.UI.Products
{
    public class ProductSinglePageQueryModel
    {
        public int ProductSellId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSlug { get; set; }
        public string Category { get; set; }
        public string CategorySlug { get; set; }
        public int SellerId { get; set; }
        public string SellerSlug { get; set; }
        public string SelleTitle { get; set; }
        public string SellerImageName  { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
        public int CityId { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public decimal priceAfterOff { get; set; }
        public int discountPercent { get; set; }
        public List<ProductUiQueryModel> RelatedProducts { get; set; }
        public List<ProductGalleryQueryModel> Gallery { get; set; }
        public List<ProductFeatureQueryModel> Features { get; set; }
        public SeoUiQueryModel Seo { get; set; }
        public List<BreadCrumb> BreadCrumbs { get; set; }

    }

}
