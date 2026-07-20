using Shared.Application;
using Shared.Application.Validations;
using Shop.Application.Contract.Order.Command;
using Shop.Domain.OrderAgg;
using Shop.Domain.OrderItemAgg;
using Shop.Domain.OrderSellerAgg;
using Shop.Domain.ProductSellAgg;
using Shop.Domain.SellerAgg;
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

        public async Task<PricesAfterApplyDiscountDto> ApplySellerDiscountAsync(int userId, int sellerId, int DiscountId, int DiscountPercent, string Title)
        {
            var Order = await _orderRepository.GetOpenOrderForUserAsync(userId);
            if (Order.OrderSellers.Any(x => x.SellerId == sellerId) == false)
                return new PricesAfterApplyDiscountDto
                {
                    Success = false,
                    Message = "فروشگاه یافت نشد!"
                };
            var seller = Order.OrderSellers.Single(x => x.SellerId == sellerId);
            seller.AddDiscount(DiscountId, DiscountPercent, Title);
            if (await _orderRepository.SaveAsync())
                return new PricesAfterApplyDiscountDto
                {
                    Success = true,
                    Message = "کد تخفیف با موفقیت اعمال شد",
                    sellerId = sellerId,
                    SellerPrice = seller.Price,
                    DiscountPrice = Order.Price - (Order.PaymentPrice - Order.PostPrice),
                    SellerPriceAfterOff = seller.PriceAfterOff,
                    PaymentPrice = Order.PaymentPrice,
                    SellerPaymentPrice = seller.PaymentPrice,
                    DiscountPercent = seller.DiscountPercent,
                    DiscountTitle = seller.DiscountTitle,
                    TolalPrice = Order.Price,
                    totlaPriceAfterOff = Order.PriceAfterOff

                };
            return new PricesAfterApplyDiscountDto
            {
                Success = false,
                Message = ValidationMessages.SystemErrorMessage
            };
        }

        public async Task<PricesAfterApplyDiscountDto?> ApplyOrderDiscountAsync(int userId, int DiscountId, int DiscountPercent, string Title)
        {

            var Order = await _orderRepository.GetOpenOrderForUserAsync(userId);
            Order.AddDiscount(DiscountId, DiscountPercent, Title);
            if (await _orderRepository.SaveAsync())
            {
                return new PricesAfterApplyDiscountDto
                {
                    Success = true,
                    Message = "کد تخفیف با موفقیت اعمال شد",
                    DiscountTitle = Title,
                    DiscountPrice = Order.Price - (Order.PaymentPrice - Order.PostPrice),
                    DiscountPercent = DiscountPercent,
                    PaymentPrice = Order.PaymentPrice,
                    TolalPrice = Order.Price,
                    totlaPriceAfterOff = Order.PriceAfterOff
                };
            }

            return new PricesAfterApplyDiscountDto
            {
                Success = false,
                Message = ValidationMessages.SystemErrorMessage
            };
        }

        public async Task<PricesAfterApplyDiscountDto> RemoveSellerDiscountAsync(int UserId, int SellerId)
        {
            var order = await _orderRepository.GetOpenOrderForUserAsync(UserId);
            var seller = order.OrderSellers.SingleOrDefault(x => x.SellerId == SellerId);
            if (seller is null)
                return new PricesAfterApplyDiscountDto
                {
                    Success = false,
                    Message = "فروشگاه یافت نشد!"
                };
            int DiscountPercent = seller.DiscountPercent;
            int DiscountId = seller.DiscountId;
            seller.RemoveDiscount();
            if (await _orderRepository.SaveAsync())
            {

                return new PricesAfterApplyDiscountDto
                {
                    Success = true,
                    Message = "کد تخفیف با موفقیت حذف شد",
                    sellerId = SellerId,
                    SellerPrice = seller.Price,
                    DiscountPrice = order.Price - (order.PaymentPrice - order.PostPrice),
                    SellerPriceAfterOff = seller.PriceAfterOff,
                    PaymentPrice = order.PaymentPrice,
                    SellerPaymentPrice = seller.PaymentPrice,
                    DiscountPercent = seller.DiscountPercent,
                    DiscountTitle = seller.DiscountTitle,
                    DiscountId = DiscountId,
                    TolalPrice = seller.Price,
                    totlaPriceAfterOff = seller.PriceAfterOff

                };
            }
            return new PricesAfterApplyDiscountDto
            {
                Success = false,
                Message = ValidationMessages.SystemErrorMessage
            };

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

                var productSellIds = cart
                    .Select(x => x.ProductSellId)
                    .Distinct()
                    .ToHashSet();

                var productSells = await _productsellRepository.GetByIdsAsync(productSellIds.ToList());

                if (!productSells.Any())
                    return new(false, "محصولی یافت نشد.");

                var productSellDict = productSells.ToDictionary(x => x.Id);

                bool hasChanges = false;

               
                foreach (var orderSeller in order.OrderSellers.ToList())
                {
                    var itemsToRemove = orderSeller.OrderItems
                        .Where(x => !productSellIds.Contains(x.ProductSellId))
                        .ToList();

                    foreach (var item in itemsToRemove)
                    {
                        orderSeller.RemoveOrderItem(item);
                        hasChanges = true;
                    }

                    if (!orderSeller.OrderItems.Any())
                    {
                        order.RemoveOrderSeller(orderSeller);
                        hasChanges = true;
                    }
                }

                // افزودن یا بروزرسانی آیتم‌ها
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
                        hasChanges = true;
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

                        if (existingItem.Price != item.Price ||
                            existingItem.PriceAfterOff != item.PriceAfterOff)
                        {
                            existingItem.ChangePrice(item.Price, item.PriceAfterOff);
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

        public async Task<PricesAfterApplyDiscountDto> RemoveOrderDiscountAsync(int userId, int orderId)
        {
            var Order = await _orderRepository.GetOpenOrderForUserAsync(userId);
            if (Order is null)
                return new PricesAfterApplyDiscountDto
                {
                    Success = false,
                    Message = "فاکتور یافت نشد!"
                };
            int DiscountId = Order.DiscountId;
            int DiscountPercent = Order.DiscountPercent;
            Order.RemoveDiscount();
            if (await _orderRepository.SaveAsync())
            {

                return new PricesAfterApplyDiscountDto
                {
                    Success = true,
                    Message = "کد تخفیف با موفقیت اعمال شد",
                    DiscountTitle = Order.DiscountTitle,
                    DiscountPrice = Order.Price - (Order.PaymentPrice - Order.PostPrice),
                    DiscountPercent = DiscountPercent,
                    PaymentPrice = Order.PaymentPrice,
                    DiscountId = DiscountId,
                    TolalPrice = Order.Price,
                    totlaPriceAfterOff = Order.PriceAfterOff

                };
            }
            return new PricesAfterApplyDiscountDto
            {
                Success = false,
                Message = ValidationMessages.SystemErrorMessage
            };
        }

   
    }
}
