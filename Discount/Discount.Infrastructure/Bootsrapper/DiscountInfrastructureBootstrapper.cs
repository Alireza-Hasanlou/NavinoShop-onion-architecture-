using Discount.Application.Boostrapper;
using Discount.Domain.OrderDiscountAgg;
using Discount.Domain.ProductDiscountAgg;
using Discount.Infrastructure.Persistence.Context;
using Discount.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Discount.Infrastructure.Bootsrapper
{
    public static class DiscountInfrastructureBootstrapper
    {
        public static void Config(IServiceCollection services, string ConnectionStrisng)
        {
            DiscountApplicationBootstrapper.Config(services);
            services.AddDbContext<DiscountContext>(option =>
            {
                option.UseSqlServer(ConnectionStrisng);
            });

            services.AddTransient<IProductDiscountRepository, ProductDiscountRepository>();
            services.AddTransient<IOrderDiscountRepository, OrderDiscountsRepository>();

        }
    }
}
