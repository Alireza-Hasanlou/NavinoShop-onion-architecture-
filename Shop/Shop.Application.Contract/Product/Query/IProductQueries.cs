using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Contract.Product.Query
{
    public interface IProductQueries
    {
        Task<ProductsForAdminPaging> GetAllProductsForAdmin(int pageId, int take, string filter ,int categoryId);
    }
}
