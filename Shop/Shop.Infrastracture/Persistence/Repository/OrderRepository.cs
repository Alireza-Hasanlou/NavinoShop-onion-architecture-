using Microsoft.EntityFrameworkCore;
using Shared.Domain.Enums;
using Shared.Insfrastructure;
using Shop.Domain.OrderAgg;
using Shop.Infrastracture.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastracture.Persistence.Repository
{
    internal class OrderRepository : GenericRepository<Order, int>, IOrderRepository
    {
        private readonly ShopContext _shopContext;

        public OrderRepository(ShopContext shopContext) : base(shopContext)
        {
            _shopContext = shopContext;
        }

        public async Task<Order> GetOpenOrderForFinalizePaymentAsync(int userId)
        {
            return await _shopContext.Orders
              .Where(x => x.UserId == userId &&
                          x.OrderStatus == OrderStatus.پرداخت_نشده)
              .Include(x => x.OrderSellers)
              .ThenInclude(x => x.OrderItems)
              .Include(x => x.OrderAddress)
              .SingleOrDefaultAsync();



        }

        public async Task<Order?> GetOpenOrderForUserAsync(int userId)
        {
            var order = await _shopContext.Orders
                .Where(x => x.UserId == userId &&
                            x.OrderStatus == OrderStatus.پرداخت_نشده)
                .Include(x => x.OrderSellers)
                .ThenInclude(x => x.OrderItems)
                .Include(x => x.OrderAddress)
                .SingleOrDefaultAsync();

            if (order != null)
                return order;

            order = new Order(userId);

            var result = await CreateAsync(order);

            if (!result.Success)
                return null;

            order = await _shopContext.Orders
                .Where(x => x.UserId == userId &&
                            x.OrderStatus == OrderStatus.پرداخت_نشده)
                .Include(x => x.OrderSellers)
                .ThenInclude(x => x.OrderItems)
                .Include(x=>x.OrderAddress)
                .SingleOrDefaultAsync();
            return order;
        }

        public async Task<bool> SetOrderPaymentTypeAsync(int userId, OrderPayment orderPayment)
        {
            int result = await _shopContext.Orders.Where(x => x.UserId == userId && x.OrderStatus == OrderStatus.پرداخت_نشده)
                .ExecuteUpdateAsync(x => x.SetProperty(p => p.OrderPayment, orderPayment));
            if (result > 0)
                return true;
            return false;
        }
    }
}
