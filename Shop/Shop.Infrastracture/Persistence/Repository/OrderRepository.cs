using Microsoft.EntityFrameworkCore;
using Shared.Application;
using Shared.Domain.Enums;
using Shared.Insfrastructure;
using Shop.Application.Contract.Order.Query;
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

        public async Task<OperationResult> CancellOrderByAdminAsync(int orderId)
        {
            await using var transaction = await _shopContext.Database.BeginTransactionAsync();

            try
            {

                var orderExists = await ExistByAsync(x => x.Id == orderId);

                if (!orderExists)
                    return new OperationResult(false, "سفارش یافت نشد");



                var hasShippedSeller = await _shopContext.OrderSellers
                    .AnyAsync(x =>
                        x.OrderId == orderId &&
                        x.Status == OrderSellerStatus.ارسال_شده);

                if (hasShippedSeller)
                    return new OperationResult(
                        false,
                        "به دلیل ارسال شدن سفارش توسط یکی از فروشگاه‌ها، امکان لغو سفارش وجود ندارد"
                    );


                
                var orderAffectedRows = await _shopContext.Orders
                    .Where(x => x.Id == orderId &&
                                (x.OrderStatus == OrderStatus.پرداخت_نشده ||
                                 x.OrderStatus == OrderStatus.پرداخت_شده))
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(
                            x => x.OrderStatus,
                            OrderStatus.لغو_شده_توسط_ادمین
                        ));

                if (orderAffectedRows == 0)
                    return new OperationResult(
                        false,
                        "وضعیت فعلی سفارش امکان لغو ندارد"
                    );



                await _shopContext.OrderSellers
                    .Where(x => x.OrderId == orderId)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(
                            x => x.Status,
                            OrderSellerStatus.لغو_شده_توسط_ادمین
                        ));


                await transaction.CommitAsync();

                return new OperationResult(true, "");
            }
            catch
            {
                await transaction.RollbackAsync();

                return new OperationResult(
                    false,
                    "خطا در لغو سفارش"
                );
            }
        }

        public async Task<OperationResult> CancellOrderByUserAsync(int orderId)
        {
            await using var transaction = await _shopContext.Database.BeginTransactionAsync();

            try
            {

                var orderExists = await ExistByAsync(x => x.Id == orderId);

                if (!orderExists)
                    return new OperationResult(false, "سفارش یافت نشد");



                var hasShippedSeller = await _shopContext.OrderSellers
                    .AnyAsync(x =>
                        x.OrderId == orderId &&
                        x.Status == OrderSellerStatus.ارسال_شده);

                if (hasShippedSeller)
                    return new OperationResult(
                        false,
                        "به دلیل ارسال شدن سفارش توسط یکی از فروشگاه‌ها، امکان لغو سفارش وجود ندارد"
                    );


                // لغو خود سفارش
                var orderAffectedRows = await _shopContext.Orders
                    .Where(x => x.Id == orderId &&
                                (x.OrderStatus == OrderStatus.پرداخت_نشده ||
                                 x.OrderStatus == OrderStatus.پرداخت_شده))
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(
                            x => x.OrderStatus,
                            OrderStatus.لغو_شده_توسط_مشتری
                        ));

                if (orderAffectedRows == 0)
                    return new OperationResult(
                        false,
                        "وضعیت فعلی سفارش امکان لغو ندارد"
                    );



                await _shopContext.OrderSellers
                    .Where(x => x.OrderId == orderId)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(
                            x => x.Status,
                            OrderSellerStatus.لغو_شده_توسط_مشتری
                        ));


                await transaction.CommitAsync();

                return new OperationResult(true, "");
            }
            catch
            {
                await transaction.RollbackAsync();

                return new OperationResult(
                    false,
                    "خطا در لغو سفارش"
                );
            }
        }

        public async Task<FactorforordercancellationQueryModel> GetFactorforordercancellationAsync(int orderId, int SellerId)
        {
            if (SellerId > 0)
            {
                return await _shopContext.Orders
                    .Where(x => x.Id == orderId)
                    .Include(o => o.OrderSellers.Where(x => x.SellerId == SellerId))
                    .ThenInclude(x => x.OrderItems)
                    .Select(x => new FactorforordercancellationQueryModel
                    {
                        OrderId = x.Id,
                        PaymentPrice = x.PaymentPriceSeller,
                        CustomerId = x.UserId
                    })
                    .SingleOrDefaultAsync();
            }
            else
            {
                return await _shopContext.Orders
                    .Where(x => x.Id == orderId)
                    .Include(o => o.OrderSellers)
                    .ThenInclude(x => x.OrderItems)
                    .Select(x => new FactorforordercancellationQueryModel
                    {
                        OrderId = x.Id,
                        PaymentPrice = x.PaymentPrice,
                        CustomerId = x.UserId
                    })
                    .SingleOrDefaultAsync();
            }


        }

        public async Task<Order> GetForEditOrderSellerStatusAsync(int orderId)
        {
            return await _shopContext.Orders.Include(x => x.OrderSellers).SingleOrDefaultAsync(x => x.Id == orderId);
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
                .Include(x => x.OrderAddress)
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
