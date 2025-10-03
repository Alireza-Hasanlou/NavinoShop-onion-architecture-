using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Site.Application.Bootstrapper;
using Site.Domain.BanerAgg;
using Site.Domain.MenuAgg;
using Site.Domain.SiteImageAgg;
using Site.Domain.SitePageAgg;
using Site.Domain.SiteServiceAgg;
using Site.Domain.SiteSettingAgg;
using Site.Domain.SliderAgg;
using Site.Infrastructure.Persistence.Context;
using Site.Infrastructure.Persistence.Repository;

namespace Site.Infrastructure.Bootstrapper
{
    public static class SiteInfrastructureBootstrapper
    {
        public static void Config(IServiceCollection services,string connectionString)
        {
            services.AddDbContext<SiteContext>(x =>
            {
                x.UseSqlServer(connectionString);
            });

            services.AddTransient<ISiteSettingRepository, SiteSettingRepository>();
            services.AddTransient<ISiteServiceRepository, SiteServiceRepository>();
            services.AddTransient<IBanerRepository, BanerRepository>();
            services.AddTransient<IMenuRepository, MenuRepository>();
            services.AddTransient<ISliderRepository,SliderRepository>();
            services.AddTransient<IImageSiteRepository,ImageSiteRepository>();
            services.AddTransient<ISitePageRepository, SitePageRepository>();

            SiteApplicationBootstraper.Config(services);    
        }
    }
}
