
using Microsoft.Extensions.DependencyInjection;
using Seos.Application.Contract.SeoService.Command;
using Seos.Application.Services;

namespace Seos.Application.Bootstrapper
{
    public static class SeoApplicationBootstrapper
    {
        public static void Config(IServiceCollection services)
        {
            services.AddTransient<ISeoCommandService, SeoService>();
        }
    }
}