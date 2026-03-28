using Microsoft.Extensions.DependencyInjection;
using Shop.Infrastracture.Bootstrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Query.Bootstrapper
{
    public static class ShopBootstrapper
    {
        public static void Config(IServiceCollection services , string ConnectionString)
        {
            ShopInfrastractureBootstrapper.Config(services, ConnectionString);  
        }
    }
}
