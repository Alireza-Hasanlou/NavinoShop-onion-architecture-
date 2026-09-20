using Query.Contract.UI.Products;
using Site.Application.Contract.MenuService.Query;

namespace NavinoShop.WebApplication.Utility.ViewModels
{
    public class BestSellersByCategoryViewModel
    {
        public List<ProductCategoryUiQueryModel> Menus { get; set; }
        public List<ProductUiQueryModel> Products { get; set; }

    }
}
