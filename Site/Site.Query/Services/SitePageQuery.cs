using Microsoft.EntityFrameworkCore;
using Shared.Application;
using Site.Application.Contract.SitePageService.Query;
using Site.Domain.SitePageAgg;

namespace Site.Query.Services
{
    internal class SitePageQuery : ISitePageQueryService
    {
        private readonly ISitePageRepository _sitePageRepository;

        public SitePageQuery(ISitePageRepository sitePageRepository)
        {
            _sitePageRepository = sitePageRepository;
        }

        public async Task<List<SitePageAdminQueryModel>> GetAllForAdmin()
        {

            return await _sitePageRepository.GetAll().Select(p => new SitePageAdminQueryModel
            {
                Active = p.Active,
                CreateDate = p.CreateDate.ToPersainDate(),
                Id = p.Id,
                Slug = p.Slug,
                Title = p.Title,
                UpdateDate = p.UpdateDate.ToPersainDate()
            }).ToListAsync();
        }

    }
}
