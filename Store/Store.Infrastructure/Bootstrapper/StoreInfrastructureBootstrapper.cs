using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Store.Application.Bootstrapper;
using Store.Domain.StoreAgg;
using Store.Domain.StoreProductAgg;
using Store.Infrastructure.Persistence.Context;
using Store.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Infrastructure.Bootstrapper
{
    public static class StoreInfrastructureBootstrapper
    {
        public static void Config(IServiceCollection services , string ConnectionString)
        {
            StoreApplicationBootstrapper.Config(services);

            services.AddDbContext<StoreContext>(option =>
            {
                option.UseSqlServer(ConnectionString);
            });

            services.AddTransient<IStoreRepository,StoreRepository>();
            services.AddTransient<IStoreProductRepository, StoreProductRepository>();
        }

       

    }
}
