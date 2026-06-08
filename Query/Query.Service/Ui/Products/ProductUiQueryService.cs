using Discount.Domain.ProductDiscountAgg;
using Discount.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using PostModule.Domain.Services;
using Query.Contract.UI.Products;
using Query.Contract.UI.Seo;
using Seos.Domain.SeoAgg;
using Shared.Domain.Enums;
using Shared.Ui;
using Shared.Ui.Enums;
using Shop.Domain.ProductAgg;
using Shop.Domain.ProductCategoryAgg;
using Shop.Domain.ProductSellAgg;
using Shop.Domain.SellerAgg;
using Shop.Infrastracture.Persistence.Context;
using System.Collections.Immutable;
using System.Runtime.Serialization.Json;


namespace Query.Service.Ui.Products
{
    internal class ProductUiQueryService : IProductUiQueryService
    {
        private readonly IProductSellRepository _productSellRepository;
        private readonly ISeoRepository _seoRepository;
        private readonly IProductCategoryRepository _categoryRepository;
        private readonly ISellerRepository _sellerRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;
        private readonly ShopContext _shopContext;
        private readonly DiscountContext _discountContext;

        public ProductUiQueryService(IProductSellRepository productSellRepository, ISeoRepository seoRepository,
            IProductCategoryRepository categoryRepository, ISellerRepository sellerRepository,
            IStateRepository stateRepository, ICityRepository cityRepository, ShopContext shopContext, DiscountContext discountContext)
        {
            _productSellRepository = productSellRepository;
            _seoRepository = seoRepository;
            _categoryRepository = categoryRepository;
            _sellerRepository = sellerRepository;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
            _shopContext = shopContext;
            _discountContext = discountContext;
        }

        public async Task<ProductSinglePageQueryModel> GetProductAsync(string sellerSlug, string productSlug)
        {
            // Validate inputs early
            if (string.IsNullOrWhiteSpace(sellerSlug) || string.IsNullOrWhiteSpace(productSlug))
                throw new ArgumentException("Seller slug and product slug are required");


            var product = await _shopContext.productSells
                .Where(x => x.Seller.Slug == sellerSlug && x.Product.Slug == productSlug && x.Amount > 0)
                .Select(x => new ProductSinglePageQueryModel
                {
                    ProductSellId = x.Id,
                    ProductId = x.ProductId,
                    SellerId = x.SellerId,
                    StateId = x.Seller.StateId,
                    CityId = x.Seller.CityId,
                    ProductSlug = x.Product.Slug,
                    SellerSlug = x.Seller.Slug,
                    SellerImageName = x.Seller.ImageName,
                    Category = x.Product.Poduct_Category_Rels
                        .OrderBy(pcr => pcr.ProductCategory.Id)
                        .Select(pcr => pcr.ProductCategory.Title)
                        .LastOrDefault() ?? string.Empty,
                    CategorySlug = x.Product.Poduct_Category_Rels
                        .OrderBy(pcr => pcr.ProductCategory.Id)
                        .Select(pcr => pcr.ProductCategory.Slug)
                        .LastOrDefault() ?? string.Empty,
                    Description = x.Product.Description ?? string.Empty,
                    SelleTitle = x.Seller.Title ?? string.Empty,
                    ProductName = x.Product.Title ?? string.Empty,
                    Price = x.Price,
                    priceAfterOff = 0,
                    Gallery = x.Product.ProductGalleries
                        .Select(g => new ProductGalleryQueryModel
                        {
                            ImageName = g.ImageName ?? string.Empty,
                            ImageAlt = g.ImageAlt ?? string.Empty
                        })
                        .ToList(),
                    Features = x.Product.ProductFreatures
                        .Select(f => new ProductFeatureQueryModel
                        {
                            Title = f.Title,
                            Value = f.Value,
                        })
                        .ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();


            if (product == null)
                return null;
            product.State = await _stateRepository.GetStateTitle(product.StateId);
            product.City = await _cityRepository.GetCityTitle(product.CityId);
            var ids = _shopContext.product_Category_Rels.Where(x => x.ProductId == product.ProductId).Select(c => c.CategoryId).ToList();
            product.RelatedProducts = await _shopContext.productSells
                .Where(x => x.Product.Poduct_Category_Rels.Any(pcr => pcr.ProductCategory.Slug == product.CategorySlug)
                            && x.ProductId != product.ProductId
                            && x.Product.Active)
                .Select(x => new ProductUiQueryModel
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    Title = x.Product.Title,
                    ImageName = x.Product.ImageName,
                    Category = x.Product.Poduct_Category_Rels
                        .OrderByDescending(pcr => pcr.ProductCategory.Id)
                        .Select(pcr => pcr.ProductCategory.Title)
                        .FirstOrDefault() ?? "بدون دسته",
                    CategorySlug = x.Product.Poduct_Category_Rels
                        .OrderByDescending(pcr => pcr.ProductCategory.Id)
                        .Select(pcr => pcr.ProductCategory.Slug)
                        .FirstOrDefault() ?? "",
                    Price = x.Price,
                    SellerSlug = x.Seller.Slug,
                    Slug = x.Product.Slug,

                })
                .Take(8)
                .AsNoTracking()
                .ToListAsync();





            var discount = await _discountContext.ProductDiscounts
                .Where(x => x.ProductId == product.ProductId
                 && x.ProductSellId == product.ProductSellId
                 && x.StartDate <= DateTime.Now
                 && x.EndDate.Date >= DateTime.Now.Date)
                .OrderByDescending(x => x.Percent)
                .FirstOrDefaultAsync();

            if (discount != null)
            {
                product.priceAfterOff = product.Price * ((decimal)discount.Percent / 100);
                product.discountPercent = discount.Percent;
            }
            var seoTitle = $"[{product.ProductName}] | [بهترین قیمت] | [{product.SelleTitle}] + [ناوینو شاپ]";
            var seo = await _seoRepository.GetSeoForUi(product.ProductId, WhereSeo.Product, seoTitle);
            product.Seo = new SeoUiQueryModel
            {
                MetaTitle = seo.MetaTitle,
                Canonical = seo.Canonical,
                IndexPage = seo.IndexPage,
                MetaDescription = seo.MetaDescription,
                MetaKeyWords = seo.MetaKeyWords,
                Schema = seo.Schema
            };

            if (!string.IsNullOrEmpty(product.CategorySlug))
            {
                List<BreadCrumb> breadCrumb = await GetBreadCrumbs(product.CategorySlug);
                breadCrumb.Add(new BreadCrumb { Title = product.ProductName, Url = "" });
                product.BreadCrumbs = breadCrumb;
            }

            return product;
        }



        public async Task<ProductUiPaging> GetProducts(int minPrice, int maxprice, ProductSort sort,
       string categorySlug = "", string sellerSlug = "", int pageId = 1, string filter = "")
        {
            int SeoOwnerId = 0;
            string SeoTitle = "ناوینوشاپ | خرید انواع محصولات دیجیتال و لوازم خانه";
            var model = new ProductUiPaging();


            IQueryable<Product> products = _shopContext.Products
                .Where(x => x.ProductSells.Any(x => x.Amount > 0) && x.Active)
                .Include(x => x.ProductSells);

            if (!string.IsNullOrEmpty(sellerSlug))
            {
                var seller = await _shopContext.Sellers
                   .Where(x => x.Slug == sellerSlug)
                     .Select(s => new SellerInfoQueryModel
                     {
                         Id = s.Id,
                         Title = s.Title,
                         AvatarImageName = s.ImageName,
                         CoverImageName = s.CoverImage,
                         ImageAlt = s.ImageAlt ?? s.Title,
                         Slug = s.Slug,
                         StateId = s.StateId,
                         CityId = s.CityId,
                         Address = s.Address,
                         InShop = EF.Functions.DateDiffDay(s.CreateDate, DateTime.Now),
                         ProductCount = s.ProductSells.Count(p => p.Active),
                         SellCount = s.ProductSells.Sum(p => p.OrderItems.Sum(oi => oi.Count))
                     })
                       .AsNoTracking()
                       .FirstOrDefaultAsync();

                if (seller == null)
                    return null;

                seller.State = await _stateRepository.GetStateTitle(seller.StateId);
                seller.City = await _cityRepository.GetCityTitle(seller.CityId);
                model.Seller = seller;
                products = products.Where(x => x.ProductSells.Any(x => x.SellerId == seller.Id));
            }
            if (maxprice > minPrice)
            {
                products = products.Where(p => p.ProductSells.Any(ps => ps.Price >= minPrice && ps.Price <= maxprice));
            }
            if (!string.IsNullOrEmpty(categorySlug))
            {
                products = products.Where(x => x.Poduct_Category_Rels
                    .Any(pcr => pcr.ProductCategory.Slug == categorySlug));

                var category = await _categoryRepository.GetBySlugAsync(categorySlug);
                if (category != null)
                {
                    SeoOwnerId = category.Id;
                    SeoTitle = $"خرید انواع {category.Title} | فروشگاه ناوینوشاپ";
                }
            }
            if (!string.IsNullOrEmpty(filter))
            {
                products = products.Where(x => x.Title.Contains(filter));
            }
            products = sort switch
            {
                ProductSort.جدیدترین => products.OrderByDescending(x => x.CreateDate),
                ProductSort.پرفروشترین => products.OrderByDescending(x => x.ProductSells.Sum(ps => ps.OrderItems.Sum(oi => oi.Count))),
                ProductSort.ارزانترین => products.OrderBy(x => x.ProductSells.Min(ps => ps.Price)),
                _ => products.OrderByDescending(x => x.CreateDate)
            };
            var totalCount = await products.CountAsync();

            // تنظیم صفحه بندی
            model.GetData(products, pageId, 12, 5);
            model.Filter = filter;
            model.categorySlug = categorySlug;
            model.ProductSort = sort;
            model.MinPrice = minPrice;
            model.MaxPrice = maxprice;
            model.BreadCrumbs = await GetBreadCrumbs(categorySlug);

            // اگر محصولی وجود ندارد
            if (totalCount == 0)
            {
                model.Products = new List<ProductUiQueryModel>();
                model.Seo = await GetSeoAsync(SeoOwnerId, SeoTitle);
                return model;
            }

            // دریافت محصولات با Select بهینه
            model.Products = await products

                .Skip(model.Skip)
                .Take(model.Take)
                .Select(p => new ProductUiQueryModel
                {
                    ProductId = p.Id,
                    Title = p.Title ?? string.Empty,
                    ImageAlt = p.ImageAlt ?? string.Empty,
                    ImageName = p.ImageName ?? string.Empty,
                    SellerTitle = string.Empty,
                    SellerSlug = string.Empty,
                    Slug = p.Slug ?? string.Empty,
                    PriceAfterOff = 0,
                    CategorySlug = p.Poduct_Category_Rels
                        .OrderByDescending(x => x.ProductCategory.Id)
                        .Select(pcr => pcr.ProductCategory.Slug)
                        .FirstOrDefault() ?? "بدون دسته",
                    Category = p.Poduct_Category_Rels
                        .OrderByDescending(x => x.ProductCategory.Id)
                        .Select(pcr => pcr.ProductCategory.Title)
                        .FirstOrDefault() ?? "بدون دسته",
                    Price = 0,
                    productSells = p.ProductSells.Where(x => x.Amount > 0)
                    .Select(x => new productSellQuery
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        Price = x.Price,
                        SellerTitle = x.Seller.Title,
                        SellerSlug = x.Seller.Slug,

                    }).ToList()
                })
                .AsNoTracking()
                .ToListAsync();

            var Discounts = await _discountContext.ProductDiscounts
                .Where(x => x.StartDate.Date <= DateTime.Now.Date
                 && x.EndDate.Date >= DateTime.Now.Date)
                .OrderByDescending(x => x.Percent)
                .ToListAsync();


            if (Discounts.Any())
            {

                foreach (var product in model.Products)
                {
                    var discount = Discounts.FirstOrDefault(x => x.ProductId == product.ProductId);
                    if (discount != null)
                    {
                        if (discount.ProductSellId > 0)
                        {
                            var productSell = product.productSells.SingleOrDefault(x => x.ProductId == product.ProductId
                            && x.Id == discount.ProductSellId);
                            if (productSell != null)
                            {
                                product.Price = productSell.Price;
                                product.SellerTitle = productSell.SellerTitle;
                                product.SellerSlug = productSell.SellerSlug;
                                product.PriceAfterOff = productSell.Price * ((decimal)discount.Percent / 100);
                                product.discountPercent = discount.Percent;
                            }


                        }
                        else if (discount.ProductSellId == 0)
                        {
                            var productSell = product.productSells.OrderBy(x => x.Price).FirstOrDefault();
                            if (productSell != null)
                            {
                                product.Price = productSell.Price;
                                product.SellerTitle = productSell.SellerTitle;
                                product.SellerSlug = productSell.SellerSlug;
                                
                                product.PriceAfterOff = productSell.Price * ((decimal)discount.Percent/100);
                                product.discountPercent = discount.Percent;
                            }

                        }
                    }
                    else
                    {
                        var productSell = product.productSells.OrderBy(x => x.Price).FirstOrDefault();
                        if (productSell != null)
                        {
                            product.Price = productSell.Price;
                            product.SellerTitle = productSell.SellerTitle;
                            product.SellerSlug = productSell.SellerSlug;
                        }

                    }

                }
            }

            model.Seo = await GetSeoAsync(SeoOwnerId, SeoTitle);

            return model;
        }




        public async Task<List<ProductUiQueryModel>> GetProductOtherSellers(int SellerId, string productSlug)
        {

            var productId = await _shopContext.Products
                .Where(x => x.Slug == productSlug
                && x.Active
                && x.ProductSells.Any())
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            if (productId == 0)
                return new List<ProductUiQueryModel>();

            return await _shopContext.productSells
                .Where(x => x.Product.Slug == productSlug && x.SellerId != SellerId)
                .OrderBy(x => x.Price)
                .Select(x => new ProductUiQueryModel
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    Title = x.Product.Title,
                    ImageName = x.Product.ImageName,
                    Category = x.Product.Poduct_Category_Rels
                        .OrderByDescending(pcr => pcr.ProductCategory.Id)
                        .Select(pcr => pcr.ProductCategory.Title)
                        .FirstOrDefault() ?? "بدون دسته",
                    CategorySlug = x.Product.Poduct_Category_Rels
                        .OrderByDescending(pcr => pcr.ProductCategory.Id)
                        .Select(pcr => pcr.ProductCategory.Slug)
                        .FirstOrDefault() ?? "",
                    Price = x.Price,
                    SellerSlug = x.Seller.Slug,
                    SellerTitle = x.Seller.Title,
                    Slug = x.Product.Slug,
                })
                .Take(8)
                .AsNoTracking()
                .ToListAsync();
        }

        #region PrivateMthods
        private async Task<SeoUiQueryModel> GetSeoAsync(int ownerId, string defaultTitle)
        {
            var seo = await _seoRepository.GetSeoForUi(ownerId, WhereSeo.ProductCategory, defaultTitle);
            return new SeoUiQueryModel
            {
                MetaTitle = seo?.MetaTitle ?? defaultTitle,
                Canonical = seo?.Canonical,
                IndexPage = seo?.IndexPage ?? true,
                MetaDescription = seo?.MetaDescription ?? defaultTitle,
                MetaKeyWords = seo?.MetaKeyWords,
                Schema = seo?.Schema
            };
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
        #endregion
    }
}

