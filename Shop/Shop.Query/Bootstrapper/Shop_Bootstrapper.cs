using Microsoft.Extensions.DependencyInjection;
using Shop.Application.Contract.Product.Query;
using Shop.Application.Contract.ProductCategory.Query;
using Shop.Application.Contract.ProductFeature.Query;
using Shop.Application.Contract.ProductGallery.Query;
using Shop.Application.Contract.Seller.Query;
using Shop.Infrastracture.Bootstrapper;
using Shop.Query.Queries;


namespace Shop.Query.Bootstrapper
{
    public static class ShopBootstrapper
    {
        public static void Config(IServiceCollection services, string ConnectionString)
        {
            ShopInfrastractureBootstrapper.Config(services, ConnectionString);

            services.AddTransient<ISellerQueries, SellerQueries>();
            services.AddTransient<IProductCategoryQueries, ProductCategoryQueries>();
            services.AddTransient<IProductFeatureQueries, ProductFeatureQueries>();
            services.AddTransient<IProductGalleryQueries, ProductGalleryQueries>();
            services.AddTransient<IProductQueries, ProductQueries>();   
        }
    }
}
