
using Microsoft.EntityFrameworkCore;
using Shared.Insfrastructure;
using Site.Application.Contract.MenuService.Command;
using Site.Domain.MenuAgg;
using Site.Infrastructure.Persistence.Context;

namespace Site.Infrastructure.Persistence.Repository;

internal class MenuRepository : GenericRepository<Menu, int>, IMenuRepository
{
    private readonly SiteContext _context;

    public MenuRepository(SiteContext context) : base(context)
    {
        _context = context;
    }

    public async Task<EditMenuCommandModel> GetForEdit(int id) =>
       await _context.Menus.Select(s => new EditMenuCommandModel
        {
            ImageAlt = s.ImageAlt,
            Id = s.Id,
            ImageFile = null,
            ImageName = s.ImageName,
            Number  =s.Number,
            Title = s.Title,
            ParentId = s.ParentId == null ? 0 : s.ParentId.Value, 
            Url = s.Url
        }).SingleOrDefaultAsync(s => s.Id == id);
}
