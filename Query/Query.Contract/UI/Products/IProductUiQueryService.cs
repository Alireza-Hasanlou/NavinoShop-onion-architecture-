using Shared.Ui.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Contract.UI.Products
{
    public interface IProductUiQueryService
    {
        Task<ProductUiPaging> GetProducts(int minPrice, int maxprice, ProductSort sort,
            string categorySlug = "", string sellerSlug = "", int pageId = 1, string filter = "");
        Task<ProductSinglePageQueryModel> GetProductAsync(string SellerSlug, string Productslug);
        Task<List<ProductUiQueryModel>> GetProductOtherSellers(int SellerId, string productSlug);
        Task<BestProductUiQueryModel> GetBestProducts();
        Task<List<ProductUiQueryModel>> GetBestSellersByCategoryAsync(string categoryName);
    }
    public class BestProductUiQueryModel
    {
        public List<ProductUiQueryModel> MostViewdProduct { get; set; }
        public List<ProductUiQueryModel> MostDiscountProduct { get; set; }
        public List<ProductUiQueryModel> ChoosenProduct { get; set; }
    }

    
}
