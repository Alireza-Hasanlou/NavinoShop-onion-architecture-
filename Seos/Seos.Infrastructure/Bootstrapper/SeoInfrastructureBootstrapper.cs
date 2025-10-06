using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Seos.Application;
using Seos.Application.Bootstrapper;
using Seos.Domain;
using Seos.Domain.SeoAgg;
using Seos.Infrastructure.Context;
using Seos.Infrastructure.Persistence.Repository;

namespace Seos.Infrastructure.Bootstrapper
{
    public static class SeoInfrastructureBootstrapper
    {
        public static void Config(IServiceCollection services, string connection)
        {
            SeoApplicationBootstrapper.Config(services);

            services.AddTransient<ISeoRepository, SeoRepository>();

            services.AddDbContext<Seo_Context>(x =>
            {
                x.UseSqlServer(connection);
            });
        }
    }
}