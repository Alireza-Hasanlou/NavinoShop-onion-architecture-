using Shared.Domain;

namespace Shop.Domain.ProductSellAgg
{
    public interface IProductSellRepository : IGenericRepository<ProductSell, int>
    {
        Task<List<ProductSell>> GetByIdsAsync(List<int> productSellIds);
        Task<bool> ProductSellHaveAmount(int id);
    }
}
