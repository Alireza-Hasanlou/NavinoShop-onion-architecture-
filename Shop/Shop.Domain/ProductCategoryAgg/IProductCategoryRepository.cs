using Shared.Domain;
using Shop.Application.Contract.ProductCategory.Commands;


namespace Shop.Domain.ProductCategoryAgg
{
    public interface IProductCategoryRepository : IGenericRepository<ProductCategory, int>
    {
        Task<ProductCategory> GetBySlugAsync(string categorySlug);
        Task<EditProductCategoryCommandModel> GetForEditAsync(int productCategoryId);
    }
}
