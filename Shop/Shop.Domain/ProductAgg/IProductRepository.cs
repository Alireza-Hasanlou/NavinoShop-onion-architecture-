using Shared.Domain;
using Shop.Application.Contract.Product.Command;


namespace Shop.Domain.ProductAgg
{
    public interface IProductRepository : IGenericRepository<Product, int>
    {
        Task<EditProductCommandModel> GetForEditAsync(int productId);
        Task<Product> GetForAddRelToCategory(int id);
    }
}
