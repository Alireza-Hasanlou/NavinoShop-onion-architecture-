using Query.Contract.Admin.Seller;
using Query.Contract.UI.Seo;
using Shared;
using Shared.Ui;
using Shared.Ui.Enums;

namespace Query.Contract.UI.Products
{
    public class ProductUiPaging : BasePaging
    {
        
        public string Filter { get; set; }
        public string categorySlug { get; set; }
        public ProductSort ProductSort { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public SellerInfoQueryModel Seller { get; set; }
        public List<ProductUiQueryModel> Products { get; set; }
        public SeoUiQueryModel Seo { get; set; }
        public List<BreadCrumb> BreadCrumbs { get; set; }
    }
    public class SellerInfoQueryModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int SellCount { get; set; }
        public string? CoverImageName { get; set; }
        public string LogoImageName  { get; set; }
        public string City { get; set; }
        public string? Address { get; set; }

    }
}
