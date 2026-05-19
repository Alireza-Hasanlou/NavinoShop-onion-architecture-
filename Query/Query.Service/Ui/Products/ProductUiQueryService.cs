using Microsoft.EntityFrameworkCore;
using Query.Contract.UI.Products;
using Seos.Domain.SeoAgg;
using Shared.Domain.Enums;
using Shared.Ui;
using Shared.Ui.Enums;
using Shop.Application.Contract.ProductCategory.Query;
using Shop.Domain.ProductAgg;
using Shop.Domain.ProductCategoryAgg;
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
        private readonly ISeoRepository _seoRepository;
        private readonly IProductCategoryRepository _categoryRepository;

        public ProductUiQueryService(IProductSellRepository productSellRepository, ISeoRepository seoRepository,
            IProductCategoryRepository categoryRepository)
        {
            _productSellRepository = productSellRepository;
            _seoRepository = seoRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<ProductUiPaging> GetProducts(int minPrice, int maxprice, ProductSort sort,
            string categorySlug = "", int sellerId = 0, int pageId = 1, string filter = "")
        {
            int SeoOwnerId = 0;
            string SeoTitle = " ناوینوشاپ | خرید  انواع محصولات دیجیتال و لوازم خانه";
            IQueryable<ProductSell> products;
            if (sellerId > 0)
            {
                products = _productSellRepository.GetAll()
                 .Where(x=>x.SellerId == sellerId)  
                .Include(x => x.Product)
                    .ThenInclude(x => x.Poduct_Category_Rels)
                    .ThenInclude(x => x.ProductCategory)
                    .Where(x => x.Product.Active)
                .Include(x => x.OrderItems)
                .Include(x => x.Seller);
            }
            else
            {
                products = _productSellRepository.GetAll()
                               .Include(x => x.Product)
                                   .ThenInclude(x => x.Poduct_Category_Rels)
                                   .ThenInclude(x => x.ProductCategory)
                                   .Where(x => x.Product.Active)
                               .Include(x => x.OrderItems)
                               .Include(x => x.Seller);
            }




            if (maxprice > minPrice)
            {
                products = products.Where(p => p.Price >= minPrice && p.Price <= maxprice);
            }


            if (!string.IsNullOrEmpty(categorySlug))
            {
                products = products.Where(x => x.Product.Poduct_Category_Rels
                    .Any(pcr => pcr.ProductCategory.Slug == categorySlug));
                var category = await _categoryRepository.GetBySlugAsync(categorySlug);
                SeoOwnerId = category.Id;
                SeoTitle = $"خرید انواع {category.Title} |  فروشگاه ناوینوشاپ";
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
                    //TODO
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
            model.BreadCrumbs = await GetBreadCrumbs(categorySlug);
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
                    Slug = p.Product.Slug,
                    PriceAfterOff = 0,
                    CategorySlug = p.Product.Poduct_Category_Rels
                    .OrderBy(x => x.ProductCategory.Id)
                        .Select(pcr => pcr.ProductCategory.Slug)
                        .LastOrDefault() ?? "بدون دسته",
                    Category = p.Product.Poduct_Category_Rels
                        .OrderBy(x => x.ProductCategory.Id)
                        .Select(pcr => pcr.ProductCategory.Title)
                        .LastOrDefault() ?? "بدون دسته",
                    Price = p.Price,
                }).ToListAsync();
            var seo = await _seoRepository.GetSeoForUi(SeoOwnerId, WhereSeo.ProductCategory, SeoTitle);
            model.Seo = new()
            {
                MetaTitle = seo.MetaTitle,
                Canonical = seo.Canonical,
                IndexPage = seo.IndexPage,
                MetaDescription = seo.MetaDescription,
                MetaKeyWords = seo.MetaKeyWords,
                Schema = seo.Schema
            };
            return model;
        }

        private async Task<List<BreadCrumb>> GetBreadCrumbs(string categorySlug)
        {
            var model = new List<BreadCrumb>();
            int num = 1;
            if (!string.IsNullOrEmpty(categorySlug))
                await GetCategory(categorySlug, 0, num);

            var lastnum = model.Select(x => x.Number).LastOrDefault();
            if (lastnum > 0)
                num = lastnum;
            model.Add(new BreadCrumb { Number = num + 2, Title = "خانه", Url = "/" });
            model.Add(new BreadCrumb { Number = num + 1, Title = "محصولات", Url = "/Products" });
            var list = model.OrderByDescending(x => x.Number).ToList();
            return list;

            async Task GetCategory(string? categorySlug, int id, int num)
            {

                ProductCategory category;
                if (id != 0)
                {
                    category = await _categoryRepository.GetByIdAsync(id);
                }
                else
                {
                    category = await _categoryRepository.GetBySlugAsync(categorySlug);
                }


                if (category != null)
                {
                    model.Add(new BreadCrumb { Number = num, Title = category.Title, Url = $"/Products/{category.Slug}" });
                    if (category.ParentId != 0)
                    {
                        num++;
                        await GetCategory("", category.ParentId, num);

                    }
                }


            }
        }
    }
}

