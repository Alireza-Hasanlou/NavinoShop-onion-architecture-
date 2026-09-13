using Azure;
using Microsoft.EntityFrameworkCore;
using PostModule.Domain.Services;
using Query.Contract.Admin.Order;
using Query.Contract.UI.UserPanel.Order;
using Query.Contract.UI.UserPanel.Seller;
using Shared.Application;
using Shared.Domain.Enums;
using Shop.Domain.OrderAgg;
using Shop.Domain.OrderSellerAgg;
using Shop.Domain.SellerAgg;
using Shop.Infrastracture.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Contract.UserService.Query;
using Users.Domain.User.Agg;
using Users.Domain.User.Agg.IRepository;

namespace Query.Service.Admin.Order
{
    public class OrderAdminQueryService : IOrderAdminQueryService
    {
        private readonly ShopContext _shopContext;
        private readonly IUserRepository _userRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;

        public OrderAdminQueryService(ShopContext shopContext, IUserRepository userRepository,
            IStateRepository stateRepository, ICityRepository cityRepository)
        {
            _shopContext = shopContext;
            _userRepository = userRepository;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
        }

        public async Task<OrderDetailsForAdminQueryModel> GetOrderDetailsForAdminAsync( int orderId)
        {
            if ( orderId <= 0)
                return null;

            var order = await _shopContext.Orders
      .AsNoTracking()
      .Where(x =>x.Id == orderId)
      .Include(x => x.OrderAddress)
      .Include(o => o.OrderSellers)
          .ThenInclude(os => os.OrderItems)
              .ThenInclude(oi => oi.ProductSell)
                  .ThenInclude(ps => ps.Product)
      .Include(o => o.OrderSellers)
          .ThenInclude(os => os.Seller)
      .SingleOrDefaultAsync();

            if (order == null)
                return null;

            var result = new OrderDetailsForAdminQueryModel
            {
                OrderId = order.Id,
                OrderAddressId = order.OrderAddressId,
                DiscountId = order.DiscountId,
                DiscountPercent = order.DiscountPercent,
                DiscountTitle = order.DiscountTitle,
                PostPrice = order.PostPrice,
                OrderDate = order.CreateDate.ToPersainDate(),
                Price = order.Price,
                PriceAfterOff = order.PriceAfterOff,
                PaymentPrice = order.PaymentPrice,
                status=order.OrderStatus,
                OrderPayment = order.OrderPayment,
                PaymentPriceSeller = order.PaymentPriceSeller,
                DiscountPrice = order.Price - order.PriceAfterOff,

                OrderSellers = order.OrderSellers.Select(s => new OrderSellersAdminQueryModel
                {
                    Id = s.Id,
                    DiscountId = s.DiscountId,
                    SellerCityId = s.Seller.CityId,
                    DiscountPercent = s.DiscountPercent,
                    PaymentPrice = s.PaymentPrice,
                    Price = s.Price,
                    DiscountTitle = s.DiscountTitle,
                    ImageName = s.Seller.ImageName,
                    PriceAfterOff = s.PriceAfterOff > 0 ? s.PriceAfterOff : s.Price,
                    PostPrice = s.PostPrice,
                    PostTitle = s.PostTitle,
                    SellerId = s.SellerId,
                    SellerName = s.Seller?.Title ?? "نامشخص",
                    DiscountPrice = s.Price - s.PriceAfterOff,
                    Items = s.OrderItems.Select(i => new OrderItemAdminQueryModel
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


            result.Address = new OrderAddressAdminQueryModel
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






        public async Task<OrdersForAdminPanelPaging> GetOrdersAsync(int orderId, int refId, int pageId, OrderStatus status, string filter = "")
        {
            var model = new OrdersForAdminPanelPaging();

            IQueryable<Shop.Domain.OrderAgg.Order> orders =
                _shopContext.Orders.Include(x => x.OrderSellers).AsNoTracking();

            if (status != OrderStatus.همه)
            {
                orders = orders.Where(x => x.OrderStatus == status);
            }
            if (refId > 0)
            {
                orders = orders.Where(x => x.Id == orderId);
            }
            if (orderId > 0)
            {
                orders = orders.Where(x => x.Id == orderId);
            }
            List<UserQueryModel> customers = new();
            var customerIds = await orders
    .Select(x => x.UserId)
    .Distinct()
    .ToListAsync();
            customers = await _userRepository.GetUsersByIds(customerIds);
            if (!string.IsNullOrWhiteSpace(filter))
            {

                var selectedIds = customers
                    .Where(x =>
                        x.FullName.Contains(filter) ||
                        x.Mobile.Contains(filter))
                    .Select(x => x.Id)
                    .ToList();

                orders = orders.Where(x =>
                    selectedIds.Contains(x.UserId));
            }

            model.GetData(orders, pageId, 5, 5);

            model.Filter = filter;
            model.status = status;
            model.refId = refId;
          

            model.Orders = await orders
                .OrderByDescending(x => x.Id)
                .Skip(model.Skip)
                .Take(model.Take)
                .Select(x => new OrderForAdminPanelQueryModel(
                    x.Id,
                    x.UserId,
                    "",
                    x.Id,
                    x.CreateDate.ToPersainDate(),
                    x.PaymentPrice,
                    x.OrderStatus))
                .ToListAsync();

            if (model.Orders.Any())
            {


                var customerDictionary = customers
                    .ToDictionary(x => x.Id);


                model.Orders = model.Orders
                    .Select(item =>
                    {
                        customerDictionary.TryGetValue(
                            item.CustomerId,
                            out var customer);

                        return item with
                        {
                            customerName = customer?.FullName ?? "بدون نام"
                        };
                    })
                    .ToList();
            }


            return model;
        }
    }
}
