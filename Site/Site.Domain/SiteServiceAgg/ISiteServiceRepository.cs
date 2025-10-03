using Shared.Domain;
using Site.Application.Contract.SiteServiceService.Command;

namespace Site.Domain.SiteServiceAgg
{
    public interface ISiteServiceRepository : IGenericRepository<SiteService, int>
    {
       Task< EditSiteServiceCommandModel> GetForEdit(int id);
    }
}
