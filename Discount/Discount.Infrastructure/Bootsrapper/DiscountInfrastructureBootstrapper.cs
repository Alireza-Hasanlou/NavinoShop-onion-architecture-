using Discount.Application.Boostrapper;
using Discount.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        }
    }
}
