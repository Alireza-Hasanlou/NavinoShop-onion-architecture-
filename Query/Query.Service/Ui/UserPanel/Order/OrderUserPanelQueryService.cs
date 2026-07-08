using Discount.Domain.OrderDiscountAgg;
using Microsoft.EntityFrameworkCore;
using PostModule.Domain.Services;
using Query.Contract.UI.UserPanel.Order;
using Shared.Domain.Enums;
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
                DiscountPrice = CalculateDiscount(order.Price, order.PriceAfterOff),

                OrderSellers = order.OrderSellers.Select(s => new OrderSellerUserPanelQueryModel
                {
                    Id = s.Id,
                    DiscountId = s.DiscountId,
                    DiscountPercent = s.DiscountPercent,
                    PaymentPrice = s.PaymentPrice,
                    Price = s.Price,
                    DiscountTitle = s.DiscountTitle,
                    PriceAfterOff = s.PriceAfterOff,
                    PostPrice = s.PostPrice,
                    SellerId = s.SellerId,
                    SellerName = s.Seller?.Title ?? "نامشخص",
                    DiscountPrice = CalculateDiscount(s.Price, s.PriceAfterOff),
                    Items = s.OrderItems.Select(i => new OrderItemQueryMoedel
                    {
                        Id = i.Id,
                        Quantity = i.Count,
                        ProductSellId = i.ProductSellId,
                        ImageName = i.ProductSell.Product.ImageName,
                        ImageAlt = i.ProductSell.Product.ImageAlt,
                        Price = i.Price,
                        PriceAfterOff = i.PriceAfterOff,
                        ProductId = i.ProductSell.ProductId,
                        ProductName = i.ProductSell.Product.Title,
                        SellerId = i.ProductSell.SellerId
                    }).ToList()
                }).ToList()
            };

            var addresses = await _userAddressRepository
                .GetAllBy(x => x.UserId == userId)
                .Select(a => new OrderAddressQueryModel
                {
                    AddressDetail = a.AddressDetail,
                    CityId = a.CityId,
                    NationalCode = a.NationalCode,
                    FullName = a.FullName,
                    Phone = a.Phone,
                    PostalCode = a.PostalCode,
                    StateId = a.StateId,
                    StateName = "",
                    CityName = ""
                })
                .ToListAsync();


            foreach (var item in addresses)
            {
                item.CityName = await _cityRepository.GetCityTitle(item.CityId);
                item.StateName = await _stateRepository.GetStateTitle(item.StateId);
            }

            result.Addresses = addresses;
            return result;
        }


        private static int CalculateDiscount(int? originalPrice, int? priceAfterOff)
        {
            if (!originalPrice.HasValue || !priceAfterOff.HasValue || priceAfterOff <= 0)
                return 0;

            var discountPrice = originalPrice.Value - priceAfterOff.Value;
            if (discountPrice < 1)
                return 0;
            return discountPrice;
        }


    }

}