using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shop.Application.Bootstrapper;
using Shop.Infrastracture.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastracture.Bootstrapper
{
    public static class ShopInfrastractureBootstrapper
    {
        public static void Config(IServiceCollection services,string ConnectionString)
        {
            ShopApplicationBootstrapper.Config(services);
            services.AddDbContext<ShopContext>(x =>
            {
                x.UseSqlServer(ConnectionString);
            });
        }
    }
}
