using Shared.Domain;

namespace Shop.Domain.ProductSellAgg
{
    public interface IProductSellRepository : IGenericRepository<ProductSell, int>
    {
        Task<bool> ProductSellHaveAmount(int id);
    }
}
