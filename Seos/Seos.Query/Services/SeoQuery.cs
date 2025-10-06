using Seos.Application.Contract;
using Seos.Application.Contract.SeoService.Query;
using Seos.Domain;
using Seos.Domain.SeoAgg;
using Shared.Domain.Enums;
using System.Threading.Tasks;

namespace Seos.Query.Services;
class SeoQuery : ISeoQueryService
{
    private readonly ISeoRepository _repository;

    public SeoQuery(ISeoRepository repository)
    {
        _repository = repository;
    }

    public async Task<SeoQueryModel> GetSeo(int ownerId, WhereSeo where, string title)
    {
        var seo =await _repository.GetSeoForUi(ownerId, where, title);
        return new(seo.MetaTitle, seo.MetaDescription, seo.MetaKeyWords, seo.IndexPage, seo.Canonical, seo.Schema);
    }
}
