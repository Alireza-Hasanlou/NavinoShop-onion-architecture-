using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Contract.Product.Query
{
    public interface IProductQueries
    {
        Task<List<ProductsForAddtoShopQueryModel>> GetProductsForAddToShop(List<int> categoryIds);
    
    }
}
