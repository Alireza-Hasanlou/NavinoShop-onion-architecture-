using Microsoft.Extensions.DependencyInjection;
using Store.Infrastructure.Bootstrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Query.Bootstrapper
{
    public static class StoreBootstrapper
    {
        public static void Config(IServiceCollection services, string ConnectionString)
        {
            StoreInfrastructureBootstrapper.Config(services, ConnectionString);
        }
    }
}
