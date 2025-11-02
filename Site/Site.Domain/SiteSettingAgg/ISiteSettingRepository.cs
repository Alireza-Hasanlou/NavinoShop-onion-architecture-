using Site.Application.Contract.SiteSettingService.Command;

namespace Site.Domain.SiteSettingAgg
{
    public interface ISiteSettingRepository
    {
       Task< UpsertSiteSetting> GetForUpsert();
       Task<SiteSetting> GetSingle();
       Task< bool> SaveAsync();
    }
}
