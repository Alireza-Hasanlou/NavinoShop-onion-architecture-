using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Contract.Admin.Products
{
    public interface IAdminProductsQueryService
    {
        Task<ProductsForAdminPaging> GetAllProductsForAdmin(int pageId, int take, string filter, int categoryId);
     
    }
}
