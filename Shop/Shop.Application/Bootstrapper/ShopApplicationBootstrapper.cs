using Microsoft.Extensions.DependencyInjection;
using Shop.Application.Commands;
using Shop.Application.Contract.Cart;
using Shop.Application.Contract.Order.Command;
using Shop.Application.Contract.OrderSeller.Command;
using Shop.Application.Contract.Product.Command;
using Shop.Application.Contract.ProductCategory.Commands;
using Shop.Application.Contract.ProductFeature.Command;
using Shop.Application.Contract.ProductGallery.Command;
using Shop.Application.Contract.ProductSell.Command;
using Shop.Application.Contract.ProductView;
using Shop.Application.Contract.Seller.Command;


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
            services.AddTransient<IProductSellCommands, ProductSellCommands>();
            services.AddTransient<ICartCommands,CartCommands>();
            services.AddTransient<IOrderCommands, OrderCommands>();
            services.AddTransient<IOrderSellerCommands, OrderSellerCommands>();
            services.AddTransient<IProductViewCommands, ProductViewCommands>();
    

        }
    }
}
