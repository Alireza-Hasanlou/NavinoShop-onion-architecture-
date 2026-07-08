using Shared.Application;
using Shared.Application.Validations;
using Shop.Application.Contract.Order.Command;
using Shop.Domain.OrderAgg;
using Shop.Domain.OrderItemAgg;
using Shop.Domain.OrderSellerAgg;
using Shop.Domain.ProductSellAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Commands
{
    internal class OrderCommands : IOrderCommands
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductSellRepository _productsellRepository;

        public OrderCommands(IOrderRepository orderRepository, IProductSellRepository productsellRepository)
        {
            _orderRepository = orderRepository;
            _productsellRepository = productsellRepository;
        }

        public async Task<OperationResultOrderDiscount> ApplySellerDiscountAsync(int userId, int sellerId, int DiscountId, int DiscountPercent,string Title)
        {
            var Order = await _orderRepository.GetOpenOrderForUserAsync(userId);
            if (Order.OrderSellers.Any(x => x.SellerId == sellerId) == false)
                return new(false,"فروشگاه یافت نشد!");
            var seller = Order.OrderSellers.Single(x => x.SellerId == sellerId);
            seller.AddDiscount(DiscountId, DiscountPercent, Title);
            if (await _orderRepository.SaveAsync())
                return new(true);
            return new(false,ValidationMessages.SystemErrorMessage);
        }

     

        public async Task<OperationResult> UpsertUserOrder(int userId, List<ShopCartViewModel> cart)
        {
            try
            {
                var order = await _orderRepository.GetOpenOrderForUserAsync(userId);

                if (order == null)
                    return new(false, "فاکتوری برای کاربر یافت نشد.");

                if (cart == null || !cart.Any())
                    return new(false, "سبد خرید خالی است.");

                var productSellIds = cart.Select(x => x.ProductSellId).Distinct().ToList();
                var productSells = await _productsellRepository
                    .GetByIdsAsync(productSellIds);


                if (!productSells.Any())
                    return new(false, "محصولی یافت نشد.");

                var productSellDict = productSells.ToDictionary(x => x.Id);
                bool hasChanges = false;

                foreach (var item in cart)
                {
                    if (!productSellDict.TryGetValue(item.ProductSellId, out var productSell))
                        continue;

                    var orderSeller = order.OrderSellers
                        .FirstOrDefault(x => x.SellerId == productSell.SellerId);

                    if (orderSeller == null)
                    {
                        orderSeller = new OrderSeller(productSell.SellerId);
                        order.AddOrderSeller(orderSeller);

                        var orderItem = new OrderItem(
                            orderSeller.Id,
                            productSell.Id,
                            item.Quantity,
                            item.Price,
                            item.PriceAfterOff,
                            productSell.Unit);

                        orderSeller.AddOrderItem(orderItem);
                        hasChanges = true;
                        continue;
                    }

                    var existingItem = orderSeller.OrderItems
                        .FirstOrDefault(x => x.ProductSellId == productSell.Id);

                    if (existingItem == null)
                    {
                        var orderItem = new OrderItem(
                            orderSeller.Id,
                            productSell.Id,
                            item.Quantity,
                            item.Price,
                            item.PriceAfterOff,
                            productSell.Unit);

                        orderSeller.AddOrderItem(orderItem);
                        hasChanges = true;
                    }
                    else
                    {
                        if (existingItem.Count != item.Quantity)
                        {
                            existingItem.ChangeCount(item.Quantity);
                            hasChanges = true;
                        }
                        if (existingItem.Price != item.Price)
                        {
                            existingItem.ChangePrice(item.Price , item.PriceAfterOff);
                            hasChanges = true;
                        }
                    }
                }

                if (!hasChanges)
                    return new(true);

                var result = await _orderRepository.SaveAsync();

                return result
                    ? new(true)
                    : new(false, "خطا در ذخیره‌سازی سفارش.");
            }
            catch (Exception ex)
            {
                return new(false, $"خطا در پردازش درخواست: {ex.Message}");
            }
        }
    }
}
