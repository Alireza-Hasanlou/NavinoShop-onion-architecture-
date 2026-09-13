using Discount.Domain.ProductDiscountAgg;
using Microsoft.EntityFrameworkCore;
using PostModule.Domain.Services;
using Query.Contract.UI.UserPanel.Order;
using Query.Contract.UI.UserPanel.Seller;
using Shared.Application;
using Shared.Domain.Enums;
using Shop.Domain.OrderAgg;
using Shop.Domain.OrderSellerAgg;
using Shop.Domain.ProductSellAgg;
using Shop.Domain.SellerAgg;
using Shop.Infrastracture.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.User.Agg.IRepository;

namespace Query.Service.Ui.UserPanel.Seller
{
    internal class SellerUserPanelQueryService : ISellerUserPanelQueries
    {
        private readonly ISellerRepository _sellerRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IProductDiscountRepository _productDiscountRepository;
        private readonly IProductSellRepository _productSellRepository;
        private readonly ShopContext _shopContext;
        private readonly IUserRepository _userRepository;


        public SellerUserPanelQueryService(ISellerRepository sellerRepository, IStateRepository stateRepository,
            ICityRepository cityRepository, IProductDiscountRepository productDiscountRepository,
            IProductSellRepository productSellRepository, ShopContext shopContext, IUserRepository userRepository
            )
        {
            _sellerRepository = sellerRepository;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
            _productDiscountRepository = productDiscountRepository;
            _productSellRepository = productSellRepository;
            _shopContext = shopContext;
            _userRepository = userRepository;
        }

        public async Task<Contract.UI.UserPanel.Seller.OrderUserPanelViewModel> GetOrderDetailsForSellerAsync(int sellerId, int userId, int orderId)
        {
            if (userId <= 0 || orderId <= 0)
                return null;

            var order = await _shopContext.Orders
      .AsNoTracking()
      .Where(x => x.UserId == userId && x.Id == orderId)
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

            var result = new Contract.UI.UserPanel.Seller.OrderUserPanelViewModel
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

            };
            result.OrderItems = order.OrderSellers.Where(x => x.SellerId == sellerId &&  x.OrderId==orderId)
                .Select(x => x.OrderItems.Select(o => new OrderItemQueryMoedel
            {
                Id = o.Id,
                ImageName = o.ProductSell.Product.ImageName,
                ImageAlt = o.ProductSell.Product.ImageAlt,
                Price = o.Price,
                PriceAfterOff = o.PriceAfterOff,
                ProductId = o.ProductSell.ProductId,
                ProductName = o.ProductSell.Product.Title,
                ProductSellId = o.ProductSellId,
                Quantity = o.Count,
                SellerId = o.ProductSell.SellerId
            }).ToList()).Single();
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
                .Include(x => x.OrderSellers)
                .ThenInclude(x => x.OrderItems)
                 .Select(x => new OrdersForUserPanelQueryService(x.Id, x.UpdateDate.ToPersainDate(), x.PaymentPrice, x.OrderStatus))
                 .ToListAsync();
            if (orders == null) return new();
            return orders;
        }

        public async Task<ProductsForSellerPaging> GetProductsForSellerAsync(int sellerId, int pageId, int take, string filter, int categoryId)
        {
            var model = new ProductsForSellerPaging();

            var products = _productSellRepository.GetAllBy(x => x.SellerId == sellerId)
                    .Include(x => x.Product)
                    .Include(x => x.OrderItems)
                    .Where(x => x.Product.Active)
                    .Distinct();


            if (!string.IsNullOrEmpty(filter))
            {
                products = products.Where(p =>

                   p.Product.Title.Contains(filter)
                || p.Product.Description.Contains(filter)
                || p.Product.Slug.Contains(filter));
            }


            model.GetData(products, pageId, take, 5);
            model.Filter = filter ?? "";

            model.products = await products
                .OrderByDescending(p => p.CreateDate)
                .Skip(model.Skip)
                .Take(model.Take)
                .Select(p => new ProductsForSellerQueryModel
                {
                    ProductId = p.ProductId,
                    ProductSellId = p.Id,
                    Title = p.Product.Title,
                    ShortDescription = p.Product.ShortDescription,
                    ImageName = p.Product.ImageName,
                    CreateDate = p.CreateDate,
                    UpdateDate = p.UpdateDate,
                    Count = p.Amount,
                    Active = p.Active,
                    Price = p.Price,
                    Weight = p.Weight,
                    SoldCount = p.OrderItems.Where(x => x.OrderSeller.Status == OrderSellerStatus.پرداخت_شده
                    || x.OrderSeller.Status == OrderSellerStatus.در_حال_آماده_سازی
                    || x.OrderSeller.Status == OrderSellerStatus.ارسال_شده).Sum(c => c.Count)

                })
                .ToListAsync();

            var ProductDiscountPercents = await _productDiscountRepository.GetProductsDiscountAsync();
            foreach (var product in model.products)
            {
                product.SellerDiscountPercent = ProductDiscountPercents
                    .FirstOrDefault(x => x.ProductId == product.ProductId && x.ProductSellId == product.ProductSellId)?.Percent ?? 0;
                product.AdminDiscountPercent = ProductDiscountPercents
                   .FirstOrDefault(x => x.ProductId == product.ProductId && x.ProductSellId == 0)?.Percent ?? 0;
            }



            return model;
        }
        public async Task<List<SellerUserPanelQueryModel>> GetSellersForUserPanel(int UserId)
        {
            var Sellers = await _sellerRepository.GetAllBy(i => i.UserId == UserId)
                .OrderByDescending(c => c.CreateDate)
                .Select(s => new SellerUserPanelQueryModel
                {

                    Title = s.Title,
                    Id = s.Id,
                    CityId = s.CityId,
                    ImageName = FileDirectories.SellerImageDirectory100 + s.ImageName,
                    Phone = s.Phone1,
                    CityName = "",
                    whyRejected = s.WhyRejected,
                    StateId = s.StateId,
                    CreateDate = s.CreateDate.ToPersainDate(),
                    SellerStatus = s.Status,

                }).ToListAsync();

            foreach (var item in Sellers)
            {

                var state = await _stateRepository.GetByIdAsync(item.StateId);
                var city = await _cityRepository.GetByIdAsync(item.CityId);
                item.CityName = $"{state.Title}_{city.Title}";

            }
            return Sellers;
        }


        public async Task<SellersOrdersPaging> GetSellersOrdersForUserPanelAsync(int SellerId, OrderSellerStatus status, int refId, int pageId, string filter = "")
        {
            var model = new SellersOrdersPaging();
            IQueryable<OrderSeller> orderSellers = _shopContext.OrderSellers.Where(x => x.SellerId == SellerId)
                .Include(x => x.Order);

            if (status != OrderSellerStatus.همه)
            {
                orderSellers = orderSellers.Where(x => x.Status == status);
            }
            List<int> ids = new List<int>();
            foreach (var item in orderSellers)
            {
                ids.Add(item.Order.UserId);
            }

            var customers = await _userRepository.GetUsersByIds(ids);

            if (!string.IsNullOrEmpty(filter))
            {
                var selectedIds = customers.Where(x => x.FullName.Contains(filter) || x.Mobile.Contains(filter)).Select(x => x.Id).ToList();

                foreach (var item in orderSellers)
                {
                    orderSellers = orderSellers.Where(x => selectedIds.Contains(item.Order.UserId));
                }

            }
            if (refId >= 1)
            {
                orderSellers = orderSellers.Where(x => x.Order.Id == refId);
            }
            model.GetData(orderSellers, pageId, 1, 5);
            model.Filter = filter;
            model.orderSellerStatus = status;
            model.RefId = refId;

            model.Orders = await orderSellers.OrderByDescending(x => x.Id).Skip(model.Skip).Take(model.Take)
                .Select(x => new SellersOrderQueryModel
                {
                    OrderId = x.OrderId,
                    CustomerName = "",
                    PayDate = x.Order.CreateDate.ToPersainDate(),
                    PaymentPrice = x.Order.PaymentPrice,
                    CustomerId = x.Order.UserId,
                    status = x.Status


                }).ToListAsync();


            foreach (var item in model.Orders)
            {
                item.CustomerName = customers.FirstOrDefault(x => x.Id == item.CustomerId).FullName ?? "بدون نام ";
            }

            return model;
        }//TODO refId For Seller
    }
}