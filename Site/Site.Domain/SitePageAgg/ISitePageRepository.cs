using Shared.Domain;
using Site.Application.Contract.SitePageService.Command;

namespace Site.Domain.SitePageAgg
{
    public interface ISitePageRepository : IGenericRepository<SitePage, int>
    {
        Task<SitePage> GetBySlug(string slug);
        Task<EditSitePageCommandModel> GetForEdit(int id);
    }
}
