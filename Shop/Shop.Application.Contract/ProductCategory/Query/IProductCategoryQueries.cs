

namespace Shop.Application.Contract.ProductCategory.Query
{
    public interface IProductCategoryQueries
    {
        Task<bool> CheckCategoryHaveParent(int id);
        Task<ProductCategoryAdminPageQueryModel> GetCategoriesForAdmin(int id);
        Task<List<ProductCategoryForAddProduct>> GetCategoriesForAddProduct();
        Task<List<ProductCategoryForAddProductSeller>> GetCategoryForAddProductSells(int id);
    }
}

