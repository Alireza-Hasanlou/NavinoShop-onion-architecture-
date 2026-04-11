using Microsoft.EntityFrameworkCore;
using Shared.Insfrastructure;
using Shop.Domain.SellerAgg;
using Shop.Infrastracture.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastracture.Persistence.Repository
{
    internal class SellerRepository : GenericRepository<Seller, int>, ISellerRepository
    {
        private readonly ShopContext _shopContext;
        public SellerRepository(ShopContext shopContext) : base(shopContext)
        {

        }
        public async Task<int> GetSellerUserIdAsync(int sellerId)
        {
            var seller = await _shopContext.Sellers.SingleOrDefaultAsync(i => i.Id == sellerId);
            return seller != null ? seller.UserId : 0;
        }
    }
}
