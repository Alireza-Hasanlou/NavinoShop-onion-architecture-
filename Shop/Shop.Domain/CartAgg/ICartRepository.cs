using Shared.Domain;

namespace Shop.Domain.CartAgg
{
    public interface ICartRepository : IGenericRepository<Cart, int>
    {
        Task<bool> ClearCartAsync(int userId);
        Task<Cart> GetCartProductAsync(int uesrId, int productId);
    }
}
