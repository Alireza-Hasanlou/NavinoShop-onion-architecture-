using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Contract.ProductCategory.Query
{
    public interface IProductCategoryQueries
    {
        Task<bool> CheckCategoryHaveParent(int id);
        ProductCategoryAdminPageQueryModel GetCategoriesForAdmin(int id);
        Task<List<ProductCategoryForAddProduct>> GetCategoriesForAddProduct();
        List<ProductCategoryForAddProductSeller> GetCategoryForAddProductSells(int id);
    }
}
}
