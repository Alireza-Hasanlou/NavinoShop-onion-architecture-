using Discount.Domain.ProductDiscountAgg;
using Microsoft.EntityFrameworkCore;
using PostModule.Domain.Services;
using Query.Contract.UI.UserPanel.Seller;
using Shared.Application;
using Shared.Domain.Enums;
using Shop.Domain.ProductSellAgg;
using Shop.Domain.SellerAgg;
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

        public SellerUserPanelQueryService(ISellerRepository sellerRepository, IStateRepository stateRepository,
            ICityRepository cityRepository, IProductDiscountRepository productDiscountRepository, IProductSellRepository productSellRepository)
        {
            _sellerRepository = sellerRepository;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
            _productDiscountRepository = productDiscountRepository;
            _productSellRepository = productSellRepository;
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
                .OrderByDescending(c=>c.CreateDate)
                .Select(s => new SellerUserPanelQueryModel
                {
                 
                    Title = s.Title,
                    Id = s.Id,
                    CityId = s.CityId,
                    ImageName = FileDirectories.SellerImageDirectory100 + s.ImageName,
                    Phone = s.Phone1,
                    CityName = "",
                    whyRejected=s.WhyRejected,
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
    }
}
