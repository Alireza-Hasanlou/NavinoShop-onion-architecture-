using Microsoft.EntityFrameworkCore;
using Query.Contract.UI.Products;
using Shared.Ui.Enums;
using Shop.Application.Contract.ProductCategory.Query;
using Shop.Domain.ProductAgg;
using Shop.Domain.ProductSellAgg;
using Shop.Infrastracture.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Service.Ui.Products
{
    internal class ProductUiQueryService : IProductUiQueryService
    {
       private readonly IProductSellRepository _productSellRepository;
        private readonly IProductCategoryQueries _productCategoryQueries;

        public ProductUiQueryService(IProductSellRepository productSellRepository, IProductCategoryQueries productCategoryQueries)
        {
            _productSellRepository = productSellRepository;
            _productCategoryQueries = productCategoryQueries;
        }

        public async Task<ProductUiPaging> GetProducts(int minPrice, int maxprice, ProductSort sort, string categorySlug = "", int pageId = 1, string filter = "")
        {
            IQueryable<ProductSell> products = _productSellRepository.GetAll()
                .Include(x => x.Product)
                    .ThenInclude(x => x.Poduct_Category_Rels)
                    .ThenInclude(x => x.ProductCategory)
                .Include(x => x.OrderItems)
                .Include(x => x.Seller);

       
          
            if (maxprice > minPrice)
            {
                products = products.Where(p => p.Price >= minPrice && p.Price <= maxprice);
            }

          
            if (!string.IsNullOrEmpty(categorySlug))
            {
                products = products.Where(x => x.Product.Poduct_Category_Rels
                    .Any(pcr => pcr.ProductCategory.Slug == categorySlug));
            }

      
            if (!string.IsNullOrEmpty(filter))
            {
                products = products.Where(x => x.Product.Title.Contains(filter));
            }

        
            switch (sort)
            {
                case ProductSort.جدیدترین:
                    products = products.OrderByDescending(x => x.CreateDate);
                    break;

                case ProductSort.پربازدیدترین:
                    // products = products.OrderByDescending(x => x.Product.ViewCount);
                    break;

                case ProductSort.پرفروشترین:
                    products = products.OrderByDescending(x => x.OrderItems.Sum(oi => oi.Count));
                    break;

                case ProductSort.ارزانترین:
                    products = products.OrderBy(x => x.Price); 
                    break;
            }

            var model = new ProductUiPaging();
            model.GetData(products, pageId, 12, 5);
            model.Filter = filter;
            model.categorySlug = categorySlug;
            model.ProductSort = sort;
            model.MinPrice = minPrice;
            model.MaxPrice = maxprice;
            model.Products = await products
                .Skip(model.Skip)
                .Take(model.Take)
                .Select(p => new ProductUiQueryModel
                {
                    Id = p.Id,
                    ProductId = p.ProductId,
                    Title = p.Product.Title,
                    ImageAlt = p.Product.ImageAlt,
                    ImageName = p.Product.ImageName,
                    SellerTitle = p.Seller.Title,
                    SellerId = p.SellerId,
                    Category = p.Product.Poduct_Category_Rels
                        .Select(pcr => pcr.ProductCategory.Title)
                        .FirstOrDefault() ?? "بدون دسته",
                    Price = p.Price,
                }).ToListAsync();

            return model;
        }
    }
}

