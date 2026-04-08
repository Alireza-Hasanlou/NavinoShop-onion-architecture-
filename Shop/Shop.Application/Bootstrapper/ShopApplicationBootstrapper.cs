using Microsoft.Extensions.DependencyInjection;
using Shop.Application.Commands;
using Shop.Application.Contract.Product.Command;
using Shop.Application.Contract.Product_Seller.Command;
using Shop.Application.Contract.ProductCategory.Commands;
using Shop.Application.Contract.ProductFeature.Command;
using Shop.Application.Contract.ProductGallery.Command;
using Shop.Application.Contract.Seller.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Bootstrapper
{
    public static class ShopApplicationBootstrapper
    {
        public static void Config(IServiceCollection services)
        {
            services.AddTransient<IProductCommands, ProductCommands>();
            services.AddTransient<IProductCategoryCommands, ProductCategoryCommands>();
            services.AddTransient<IProductFeatureCommands, ProductFreatureCommands>();
            services.AddTransient<IProductGalleryCommands, ProductGalleryCommands>();
            services.AddTransient<ISellerCommands, SellerCommands>();
            services.AddTransient<IProduct_Seller_Commands, ProductSellerCommands>();
            services.AddTransient<ISellerCommands, SellerCommands>();

        }
    }
}
