using Microsoft.EntityFrameworkCore;
using Shared.Domain.Enums;
using Shared.Insfrastructure;
using Shop.Domain.OrderSellerAgg;
using Shop.Infrastracture.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastracture.Persistence.Repository
{
    internal class OrderSellerRepository : GenericRepository<OrderSeller, int>, IOrderSellerRepository
    {
        private readonly ShopContext _shopContext;

        public OrderSellerRepository(ShopContext shopContext) : base(shopContext)
        {
            _shopContext = shopContext;
        }

        public async Task<int> ChangeOrderSellerStatusAsync(
     int sellerId,
     int orderId,
     OrderSellerStatus status)
        {
            await using var transaction =
                await _shopContext.Database.BeginTransactionAsync();

            try
            {
                
                var result = await _shopContext.OrderSellers
                    .Where(x =>
                        x.OrderId == orderId &&
                        x.SellerId == sellerId)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(x => x.Status, status));

                if (result == 0)
                {
                    await transaction.RollbackAsync();
                    return 0;
                }

           
                if (status == OrderSellerStatus.لغو_شده_توسط_فروشنده)
                {
               
                    var hasActiveSeller = await _shopContext.OrderSellers
                        .AnyAsync(x =>
                            x.OrderId == orderId &&
                            x.Status != OrderSellerStatus.لغو_شده_توسط_مشتری &&
                            x.Status != OrderSellerStatus.لغو_شده_توسط_فروشنده &&
                            x.Status != OrderSellerStatus.لغو_شده_توسط_ادمین);

                    if (!hasActiveSeller)
                    {
                        await _shopContext.Orders
                            .Where(x => x.Id == orderId)
                            .ExecuteUpdateAsync(setters => setters
                                .SetProperty(
                                    x => x.OrderStatus,
                                    OrderStatus.لغو_شده_توسط_ادمین));
                    }
                }

                await transaction.CommitAsync();

                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
