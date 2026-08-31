using Discount.Domain.OrderDiscountAgg;
using Microsoft.EntityFrameworkCore;
using PostModule.Domain.Services;
using Query.Contract.UI.UserPanel.Order;
using Shared.Application;
using Shared.Domain.Enums;
using Shop.Domain.OrderAgg;
using Shop.Domain.ProductSellAgg;
using Shop.Infrastracture.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Users.Domain.User.Agg.IRepository;

namespace Query.Service.Ui.UserPanel.Order
{
    internal class OrderUserPanelQueryService : IOrderUserPanelQueryService
    {
        private readonly ShopContext _shopContext;
        private readonly IUserAddressRepository _userAddressRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IOrderDiscountRepository _discountRepository;

        public OrderUserPanelQueryService(ShopContext shopContext, IUserAddressRepository userAddressRepository, IStateRepository stateRepository,
            ICityRepository cityRepository, IOrderDiscountRepository discountRepository)
        {
            _shopContext = shopContext;
            _userAddressRepository = userAddressRepository;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
            _discountRepository = discountRepository;
        }

        public async Task<int> CalculateOrdersellerWeightAsync(int ordersellerId)
        {
            if (ordersellerId <= 0)
                return 0;

            try
            {
                var query = _shopContext.OrderSellers
                    .AsNoTracking()
                    .Where(x => x.Id == ordersellerId)
                    .SelectMany(x => x.OrderItems)
                    .Where(x => x.ProductSell != null);


                var totalWeight = await query.SumAsync(x => x.ProductSell.Weight);
                return totalWeight;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        public async Task<OrderUserPanelViewModel?> GetOrderAsync(int userId)
        {
            if (userId <= 0)
                return null;

            var order = await _shopContext.Orders
      .AsNoTracking()
      .Include(o => o.OrderSellers)
          .ThenInclude(os => os.OrderItems)
              .ThenInclude(oi => oi.ProductSell)
                  .ThenInclude(ps => ps.Product)
      .Include(o => o.OrderSellers)
          .ThenInclude(os => os.Seller)
      .FirstOrDefaultAsync(o => o.UserId == userId &&
                               o.OrderStatus == OrderStatus.پرداخت_نشده);

            if (order == null)
                return null;
            if (order.DiscountId > 0)
            {
                var res = await _discountRepository.DiscountIsValidAsync(order.DiscountId);
                if (!res)
                    order.RemoveDiscount();
            }


            var result = new OrderUserPanelViewModel
            {
                OrderId = order.Id,
                OrderAddressId = order.OrderAddressId,
                DiscountId = order.DiscountId,
                DiscountPercent = order.DiscountPercent,
                DiscountTitle = order.DiscountTitle,
                PostPrice = order.PostPrice,
                Price = order.Price,
                PriceAfterOff = order.PriceAfterOff,
                PaymentPrice = order.PaymentPrice,
                OrderPayment = order.OrderPayment,
                PaymentPriceSeller = order.PaymentPriceSeller,
                DiscountPrice = order.Price - order.PriceAfterOff,

                OrderSellers = order.OrderSellers.Select(s => new OrderSellerUserPanelQueryModel
                {
                    Id = s.Id,
                    DiscountId = s.DiscountId,
                    SellerCityId = s.Seller.CityId,
                    DiscountPercent = s.DiscountPercent,
                    PaymentPrice = s.PaymentPrice,
                    Price = s.Price,
                    DiscountTitle = s.DiscountTitle,
                    PriceAfterOff = s.PriceAfterOff > 0 ? s.PriceAfterOff : s.Price,
                    PostPrice = s.PostPrice,
                    PostTitle = s.PostTitle,
                    SellerId = s.SellerId,
                    SellerName = s.Seller?.Title ?? "نامشخص",
                    DiscountPrice = s.Price - s.PriceAfterOff,
                    Items = s.OrderItems.Select(i => new OrderItemQueryMoedel
                    {
                        Id = i.Id,
                        Quantity = i.Count,
                        ProductSellId = i.ProductSellId,
                        ImageName = i.ProductSell.Product.ImageName,
                        ImageAlt = i.ProductSell.Product.ImageAlt,
                        Price = i.Price,
                        PriceAfterOff = i.PriceAfterOff > 0 ? i.PriceAfterOff : i.Price,
                        ProductId = i.ProductSell.ProductId,
                        ProductName = i.ProductSell.Product.Title,
                        SellerId = i.ProductSell.SellerId
                    }).ToList()
                }).ToList()
            };
            foreach (var orderSeller in order.OrderSellers)
            {
                if (orderSeller.DiscountId > 0)
                {
                    var res = await _discountRepository.DiscountIsValidAsync(orderSeller.DiscountId);
                    if (!res)
                        orderSeller.RemoveDiscount();
                }
            }

            var defaultAddress = await _userAddressRepository.GetDefaultAddressAsync(userId);
            result.Address = new OrderAddressQueryModel
            {
                FullName = defaultAddress.FullName,
                AddressDetail = defaultAddress.AddressDetail,
                CityId = defaultAddress.CityId,
                StateId = defaultAddress.StateId,
                CityName = await _cityRepository.GetCityTitle(defaultAddress.CityId),
                StateName = await _stateRepository.GetStateTitle(defaultAddress.StateId),
                NationalCode = defaultAddress.NationalCode,
                Phone = defaultAddress.Phone,
                PostalCode = defaultAddress.PostalCode
            };

            return result;
        }

        public async Task<OrderUserPanelViewModel> GetOrderDetailsAsync(int userId, int orderId)
        {
            if (userId <= 0  || orderId <=0)
                return null;

            var order = await _shopContext.Orders
      .AsNoTracking()
      .Where(x=>x.UserId==userId && x.Id ==orderId)
      .Include(x=>x.OrderAddress)
      .Include(o => o.OrderSellers)
          .ThenInclude(os => os.OrderItems)
              .ThenInclude(oi => oi.ProductSell)
                  .ThenInclude(ps => ps.Product)
      .Include(o => o.OrderSellers)
          .ThenInclude(os => os.Seller)
      .SingleOrDefaultAsync();

            if (order == null)
                return null;
           
            var result = new OrderUserPanelViewModel
            {
                OrderId = order.Id,
                OrderAddressId = order.OrderAddressId,
                DiscountId = order.DiscountId,
                DiscountPercent = order.DiscountPercent,
                DiscountTitle = order.DiscountTitle,
                PostPrice = order.PostPrice,
                Price = order.Price,
                PriceAfterOff = order.PriceAfterOff,
                PaymentPrice = order.PaymentPrice,
                OrderPayment = order.OrderPayment,
                PaymentPriceSeller = order.PaymentPriceSeller,
                DiscountPrice = order.Price - order.PriceAfterOff,

                OrderSellers = order.OrderSellers.Select(s => new OrderSellerUserPanelQueryModel
                {
                    Id = s.Id,
                    DiscountId = s.DiscountId,
                    SellerCityId = s.Seller.CityId,
                    DiscountPercent = s.DiscountPercent,
                    PaymentPrice = s.PaymentPrice,
                    Price = s.Price,
                    DiscountTitle = s.DiscountTitle,
                    ImageName=s.Seller.ImageName,
                    PriceAfterOff = s.PriceAfterOff > 0 ? s.PriceAfterOff : s.Price,
                    PostPrice = s.PostPrice,
                    PostTitle = s.PostTitle,
                    SellerId = s.SellerId,
                    SellerName = s.Seller?.Title ?? "نامشخص",
                    DiscountPrice = s.Price - s.PriceAfterOff,
                    Items = s.OrderItems.Select(i => new OrderItemQueryMoedel
                    {
                        Id = i.Id,
                        Quantity = i.Count,
                        ProductSellId = i.ProductSellId,
                        ImageName = i.ProductSell.Product.ImageName,
                        ImageAlt = i.ProductSell.Product.ImageAlt,
                        Price = i.Price,
                        PriceAfterOff = i.PriceAfterOff > 0 ? i.PriceAfterOff : i.Price,
                        ProductId = i.ProductSell.ProductId,
                        ProductName = i.ProductSell.Product.Title,
                        SellerId = i.ProductSell.SellerId
                    }).ToList()
                }).ToList()
            };
          
    
            result.Address = new OrderAddressQueryModel
            {
                FullName = order.OrderAddress.FullName,
                AddressDetail = order.OrderAddress.AddressDetail,
                CityId = order.OrderAddress.CityId,
                StateId = order.OrderAddress.StateId,
                CityName = await _cityRepository.GetCityTitle(order.OrderAddress.CityId),
                StateName = await _stateRepository.GetStateTitle(order.OrderAddress.StateId),
                NationalCode = order.OrderAddress.NationalCode,
                Phone = order.OrderAddress.Phone,
                PostalCode = order.OrderAddress.PostalCode
            };

            return result;
        }

        public async Task<List<OrdersForUserPanelQueryService>> GetOrdersAsync(int userId)
        {
            var orders = await _shopContext.Orders.Where(u => u.UserId == userId)
                .Include(x=>x.OrderSellers)
                .ThenInclude(x=>x.OrderItems)
                 .Select(x => new OrdersForUserPanelQueryService(x.Id, x.UpdateDate.ToPersainDate(), x.PaymentPrice,x.OrderStatus))
                 .ToListAsync();
            if (orders == null) return new();
            return orders;
        }

        public Task<int> GetUserCityAsync(int userId)
        {
            return _userAddressRepository.GetUserCityAsync(userId);
        }
    }

}