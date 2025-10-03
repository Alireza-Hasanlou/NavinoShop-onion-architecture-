using Microsoft.Extensions.DependencyInjection;
using Site.Application.Contract.BanerService.Query;
using Site.Application.Contract.ImageSiteService.Query;
using Site.Application.Contract.MenuService.Query;
using Site.Application.Contract.SitePageService.Query;
using Site.Application.Contract.SiteServiceService.Query;
using Site.Application.Contract.SiteSettingService.Query;
using Site.Application.Contract.SliderService.Query;
using Site.Infrastructure.Bootstrapper;
using Site.Query.Services;
namespace Site.Query.Bootstrapper
{
    public static class Site_Bootstrapper
    {
        public static void Config(IServiceCollection services, string connectionString)
        {
            SiteInfrastructureBootstrapper.Config(services, connectionString);

            services.AddScoped<IBanerQueryService,BanerQuery>();    
            services.AddScoped<IMenuQueryService,MenuQuery>();    
            services.AddScoped<ISliderQueryservice,SliderQuery>();    
            services.AddScoped<ISiteSettingQueryService, SiteSettingQuery>();    
            services.AddScoped<ISiteServiceQuery,SiteServiceQuery>();
            services.AddScoped<ISitePageQueryService,SitePageQuery>();
            services.AddScoped<IImageSiteQueryService, ImageSiteQuery>();
        }
    }
}
