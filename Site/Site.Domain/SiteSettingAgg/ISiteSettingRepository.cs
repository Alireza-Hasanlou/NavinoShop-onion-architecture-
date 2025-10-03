using Site.Application.Contract.SiteSettingService.Command;

namespace Site.Domain.SiteSettingAgg
{
    public interface ISiteSettingRepository
    {
       Task< UbsertSiteSetting> GetForUbsert();
       Task<SiteSetting> GetSingle();
       Task< bool> SaveAsync();
    }
}
