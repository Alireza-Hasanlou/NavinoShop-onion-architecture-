using Shared.Domain;
using Seos.Application.Contract.SeoService.Command;
using Shared.Domain.Enums;

namespace Seos.Domain.SeoAgg
{
    public interface ISeoRepository : IGenericRepository<Seo,int>
    {
        CreateSeoCommandModel GetSeoForUbsert(int ownerId, WhereSeo where);
        Seo GetSeo(int ownerId, WhereSeo where);
        Task<Seo> GetSeoForUi(int ownerId, WhereSeo where,string title);
    }
}
