using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shop.Application.Bootstrapper;
using Shop.Domain.ProductAgg;
using Shop.Domain.ProductCategoryAgg;
using Shop.Domain.ProductFreatureAgg;
using Shop.Domain.ProductGalleryAgg;
using Shop.Domain.ProductSellAgg;
using Shop.Domain.Relations.ProductCategoryRel;
using Shop.Domain.SellerAgg;
using Shop.Infrastracture.Persistence.Context;
using Shop.Infrastracture.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastracture.Bootstrapper
{
    public static class ShopInfrastractureBootstrapper
    {
        public static void Config(IServiceCollection services, string ConnectionString)
        {
            ShopApplicationBootstrapper.Config(services);
            services.AddDbContext<ShopContext>(x =>
            {
                x.UseSqlServer(ConnectionString);
            });

            services.AddTransient<ISellerRepository, SellerRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddTransient<IProductFeatureRepository , ProductFreatureRepository>();
            services.AddTransient<IProduct_Category_Repository, Product_Category_Repository>();
            services.AddTransient<IProductGalleryRepository, ProductGalleryRepository>();
            services.AddTransient<IProductSellRepository, ProductSellRepository>(); 
        }
    }
}
