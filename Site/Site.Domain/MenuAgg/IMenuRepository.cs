using Shared.Domain;
using Site.Application.Contract.MenuService.Command;

namespace Site.Domain.MenuAgg
{
    public interface IMenuRepository : IGenericRepository<Menu,int>
    {
       Task< EditMenuCommandModel> GetForEdit(int id);
    }
}
