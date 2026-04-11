using Microsoft.Extensions.DependencyInjection;
using Store.Application.Bootstrapper;
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
        }
             
    }
}
