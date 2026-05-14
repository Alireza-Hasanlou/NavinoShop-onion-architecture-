using Shared;
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
       Task< ProductUiPaging> GetProducts(int minPrice, int maxprice, ProductSort sort, string categorySlug = "", int pageId = 1, string filter = "");
    }

    public class ProductUiPaging : BasePaging
    {
        
        public string Filter { get; set; }
        public string categorySlug { get; set; }
        public ProductSort ProductSort { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public List<ProductUiQueryModel> Products { get; set; }
    }

    public class CategoryTreeItemForUi
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ParentId { get; set; }
        public int Level { get; set; }
        public bool IsChecked { get; set; }
        public bool HasChildren { get; set; }
        public List<CategoryTreeItemForUi> Children { get; set; } = new List<CategoryTreeItemForUi>();
    }

    public class ProductUiQueryModel
    {

        public int Id { get; set; }
        public int ProductId { get; set; }
        public int SellerId { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string SellerTitle { get; set; }
        public int Price { get; set; }
        public int PriceAfterOff { get; set; }
        public string ImageName { get; set; }
        public string ImageAlt { get; set; }
    }

}
