using Microsoft.Extensions.DependencyInjection;
using Store.Application.Commands;
using Store.Application.Contract.Store.Command;
using Store.Application.Contract.StoreProduct.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Bootstrapper
{
    public static class StoreApplicationBootstrapper
    {
        public static void Config(IServiceCollection services)
        {
            services.AddTransient<IStoreCommands, StoreCommands>();
            services.AddTransient<IStoreProductCommands, StoreProductCommands>();

        }
    }
}
