using Microsoft.Extensions.DependencyInjection;
using Seos.Application.Contract;
using Seos.Application.Contract.SeoService.Query;
using Seos.Infrastructure;
using Seos.Infrastructure.Bootstrapper;
using Seos.Query.Services;

namespace Seos.Query.Bootstrapper
{
    public static class Seo_Bootstrapper
    {
        public static void Config(IServiceCollection services, string connection)
        {
            services.AddTransient<ISeoQueryService, SeoQuery>();
            SeoInfrastructureBootstrapper.Config(services, connection);
        }
    }
}