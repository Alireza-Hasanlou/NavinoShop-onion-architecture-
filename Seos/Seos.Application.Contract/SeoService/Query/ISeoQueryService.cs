using Shared.Domain.Enums;

namespace Seos.Application.Contract.SeoService.Query;
public interface ISeoQueryService
{
    Task<SeoQueryModel> GetSeo(int ownerId, WhereSeo where, string title);
}
