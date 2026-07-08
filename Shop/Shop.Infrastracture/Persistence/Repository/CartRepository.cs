using Microsoft.EntityFrameworkCore;
using Shared.Insfrastructure;
using Shop.Domain.CartAgg;
using Shop.Infrastracture.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastracture.Persistence.Repository
{
    internal class CartRepository :GenericRepository<Cart,int >, ICartRepository
    {
        private readonly ShopContext _shopContext;

        public CartRepository(ShopContext context) : base(context)
        {
            _shopContext = context;
        }

        public async Task<bool> ClearCartAsync(int userId)
        {
            try
            {
                var deletedCount = await _shopContext.Carts
                    .Where(x => x.UserId == userId)
                    .ExecuteDeleteAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Task<Cart> GetCartProductAsync(int uesrId, int productId)
        {
            return _shopContext.Carts.SingleOrDefaultAsync(x=>x.UserId==uesrId&& x.ProductSellId==productId);   
        }

  
    }
}
