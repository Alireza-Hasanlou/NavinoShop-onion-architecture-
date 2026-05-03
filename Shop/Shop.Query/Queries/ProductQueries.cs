using Microsoft.EntityFrameworkCore;
using Shared.Domain.Enums;
using Shop.Application.Contract.Product.Query;
using Shop.Domain.OrderAgg;
using Shop.Domain.OrderItemAgg;
using Shop.Domain.OrderSellerAgg;
using Shop.Domain.ProductAgg;
using Shop.Domain.ProductCategoryAgg;
using Shop.Domain.ProductSellAgg;
using Shop.Domain.Relations.ProductCategoryRel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Query.Queries
{
    internal class ProductQueries : IProductQueries
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProduct_Category_Repository _product_Category_Repository;
        private readonly IProductSellRepository _productSellRepository;
        private readonly IOrderItemRepository _orderItemReposotory;

        public ProductQueries(IProductRepository productRepository, IProductCategoryRepository productCategoryRepository,
            IProduct_Category_Repository product_Category_Repository, IProductSellRepository productSellRepository, IOrderItemRepository orderItemRepository)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _product_Category_Repository = product_Category_Repository;
            _productSellRepository = productSellRepository;
            _orderItemReposotory = orderItemRepository;
        }

        public async Task<ProductsForAdminPaging> GetAllProductsForAdmin(int pageId, int take, string filter, int categoryId)
        {
            var model = new ProductsForAdminPaging();
            var products = _productRepository.GetAll().Distinct();


            if (categoryId > 0)
            {
                var category = await _productCategoryRepository.GetByIdAsync(categoryId);
                if (category == null)
                    return new ProductsForAdminPaging();

                var productIds = _product_Category_Repository.GetAllBy(x => x.CategoryId == categoryId)
                    .Select(x => x.ProductId);

                products = products.Where(p => productIds.Contains(p.Id));
                model.Title = $"محصولات در دسته بندی {category.Title}";
                model.CategoryId = categoryId;
            }


            if (!string.IsNullOrEmpty(filter))
            {
                products = products.Where(p =>

                   p.Title.Contains(filter)
                || p.Description.Contains(filter)
                || p.Slug.Contains(filter));
            }


            model.GetData(products, pageId, take, 5);
            model.Filter = filter ?? "";

            model.products = await products
                .OrderByDescending(p => p.CreateDate)
                .Skip(model.Skip)
                .Take(model.Take)
                .Select(p => new ProductsForAdminQueryModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    ShortDescription = p.ShortDescription,
                    ImageName = p.ImageName,
                    CreateDate = p.CreateDate,
                    UpdateDate = p.UpdateDate,
                    Slug = p.Slug,
                    Active = p.Active,
                    Weight = p.Weight,
                })
                .ToListAsync();

            return model;
        }
        
        public Task<List<ProductsForAddtoShopQueryModel>> GetProductsForAddToShop(List<int> categoryIds)
        {
            return _productRepository.GetProductsForAddToShop(categoryIds);
        }

        public async Task<ProductsForSellerPaging> GetProductsForSellerAsync(int sellerId, int pageId, int take, string filter, int categoryId)
        {
            var model = new ProductsForSellerPaging();

            var products = _productSellRepository.GetAllBy(x => x.SellerId == sellerId)
                    .Include(x => x.Product)
                    .Include(x=>x.OrderItems)
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
                    Id = p.Id,
                    Title = p.Product.Title,
                    ShortDescription = p.Product.ShortDescription,
                    ImageName = p.Product.ImageName,
                    CreateDate = p.CreateDate,
                    UpdateDate = p.UpdateDate,
                    Slug = p.Product.Title,
                    Active = p.Active,
                    Price=p.Price,
                    Weight = p.Weight,
                    SoldCount= p.OrderItems.Where(x=>x.OrderSeller.Status== OrderSellerStatus.پرداخت_شده
                    || x.OrderSeller.Status== OrderSellerStatus.در_حال_آماده_سازی
                    || x.OrderSeller.Status == OrderSellerStatus.ارسال_شده ).Sum(c=>c.Count)

                })
                .ToListAsync();


            return model;
        }
    }
}

