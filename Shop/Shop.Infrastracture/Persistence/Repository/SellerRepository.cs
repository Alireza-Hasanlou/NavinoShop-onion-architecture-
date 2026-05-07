using Microsoft.EntityFrameworkCore;
using Shared.Insfrastructure;
using Shop.Domain.SellerAgg;
using Shop.Infrastracture.Persistence.Context;


namespace Shop.Infrastracture.Persistence.Repository
{
    internal class SellerRepository : GenericRepository<Seller, int>, ISellerRepository
    {
        private readonly ShopContext _shopContext;
        public SellerRepository(ShopContext shopContext) : base(shopContext)
        {
            _shopContext = shopContext;
        }
        public async Task<int> GetSellerUserIdAsync(int sellerId)
        {
            var seller = await _shopContext.Sellers.SingleOrDefaultAsync(i => i.Id == sellerId);
            return seller != null ? seller.UserId : 0;
        }

        public string GetTitleById(int sellerId)
        {
            return _shopContext.Sellers.Single(x => x.Id == sellerId).Title;
        }
    }
}
