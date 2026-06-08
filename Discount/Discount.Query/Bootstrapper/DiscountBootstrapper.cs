using Discount.Application.Contract.OrderDiscounts.Query;
using Discount.Infrastructure.Bootsrapper;
using Discount.Query.Queries;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Query.Bootstrapper
{
    public static class DiscountBootstrapper
    {
        public static void Config(IServiceCollection services, string ConnectionString)
        {
            DiscountInfrastructureBootstrapper.Config(services, ConnectionString);
            services.AddTransient<IOrderDiscountsQueries, OrderDiscountsQueries>();


        }

    }
}
