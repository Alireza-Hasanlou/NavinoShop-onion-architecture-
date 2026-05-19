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
            string categorySlug = "", int sellerId = 0, int pageId = 1, string filter = "");
    }

}
