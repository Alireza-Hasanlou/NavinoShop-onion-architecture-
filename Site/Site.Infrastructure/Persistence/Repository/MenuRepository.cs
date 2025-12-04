
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Enums;
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

    public async Task<bool> ExistMainMenu(MenuStatus status)
    {
        if (status == MenuStatus.منوی_گروه_محصولات)
        {
            var mainMenu = await _context.Menus.FirstOrDefaultAsync(x => x.Status == status);
            if (mainMenu == null)
                return false;
            return true;
        }
        return false;
    }

    public async Task<EditMenuCommandModel> GetForEdit(int id)
    {
       

        var Menu = await _context.Menus.Select(s => new EditMenuCommandModel
        {
            ImageAlt = s.ImageAlt,
            Id = s.Id,
            ImageFile = null,
            ImageName = s.ImageName,
            Number = s.Number,
            Title = s.Title,
            ParentId = s.ParentId == null ? 0 : s.ParentId.Value,
            Url = s.Url,
            Status = s.Status
        }).SingleOrDefaultAsync(s => s.Id == id);

        if (Menu == null)
            return new();
        return Menu;
    }

}
