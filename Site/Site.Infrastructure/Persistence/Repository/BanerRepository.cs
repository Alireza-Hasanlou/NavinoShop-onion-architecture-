using Microsoft.EntityFrameworkCore;
using Shared.Insfrastructure;
using Site.Application.Contract.BanerService.Command;
using Site.Domain.BanerAgg;
using Site.Infrastructure.Persistence.Context;

namespace Site.Infrastructure.Persistence.Repository;

internal class BanerRepository : GenericRepository<Baner, int>, IBanerRepository
{
    private readonly SiteContext _context;

    public BanerRepository(SiteContext context) : base(context)
    {
        _context = context;
    }

    public async Task< EditBanerCommandModel> GetForEdit(int id) =>
      await  _context.Baners.Select(s => new EditBanerCommandModel
        {
            ImageAlt = s.ImageAlt,
            Id = s.Id,
            ImageFile = null,
            ImageName = s.ImageName,
            Url = s.Url
        }).SingleOrDefaultAsync(s => s.Id == id);
}
