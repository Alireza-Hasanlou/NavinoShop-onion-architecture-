using Shared.Domain;
using Shop.Application.Contract.Product.Command;
using Shop.Application.Contract.Product.Query;

namespace Shop.Domain.ProductAgg
{
    public interface IProductRepository : IGenericRepository<Product, int>
    {
        Task<EditProductCommandModel> GetForEditAsync(int productId);
        Task<Product> GetForAddRelToCategory(int id);
        Task<List<ProductsForAddtoShopQueryModel>> GetProductsForAddToShop(List<int> categoryIds);
        Task<Product> GetWithCategoryRel(int id);
    }
}
