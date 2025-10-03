using Microsoft.Extensions.DependencyInjection;
using Site.Application.Contract.BanerService.Command;
using Site.Application.Contract.ImageSiteService.Command;
using Site.Application.Contract.MenuService.Command;
using Site.Application.Contract.SitePageService.Command;
using Site.Application.Contract.SiteServiceService.Command;
using Site.Application.Contract.SiteSettingService.Command;
using Site.Application.Contract.SliderService.Command;
using Site.Application.Services;


namespace Site.Application.Bootstrapper
{
    public static class SiteApplicationBootstraper
    {
        public static void Config(IServiceCollection services)
        {
            services.AddScoped<IBanerCommandService, BanerService>();
            services.AddScoped<IMenuCommandService, MenuService>();
            services.AddScoped<ISiteServiceCommandService, SiteServiceService>();
            services.AddScoped<ISiteSettingService, SiteSettingService>();
            services.AddScoped<ISliderCommandService, SliderService>();
            services.AddScoped<ISitePageCommandService, SitePageService>();
            services.AddScoped<IImageSiteCommandService, ImageSiteService>();
        }
    }
}
