
using Microsoft.EntityFrameworkCore;
using Shared.Insfrastructure;
using Site.Application.Contract.SitePageService.Command;
using Site.Domain.SitePageAgg;
using Site.Infrastructure.Persistence.Context;

namespace Site.Infrastructure.Persistence.Repository;

internal class SitePageRepository : GenericRepository<SitePage, int>, ISitePageRepository
{
    private readonly SiteContext _context;

    public SitePageRepository(SiteContext context) : base(context)
    {
        _context = context;
    }

    public async Task<SitePage> GetBySlug(string slug)
    {
        return  await _context.SitePages.SingleOrDefaultAsync(s => s.Slug.Trim().ToLower() == slug.Trim().ToLower());
    }

    public async Task<EditSitePageCommandModel> GetForEdit(int id) =>
    await _context.SitePages.Select(s => new EditSitePageCommandModel
    {
        Id = s.Id,
        Slug = s.Slug,
        Text = s.Description,
        Title = s.Title
    }).SingleOrDefaultAsync(s => s.Id == id);
}
