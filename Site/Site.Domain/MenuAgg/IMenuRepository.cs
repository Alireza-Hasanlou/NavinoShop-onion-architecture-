using Shared.Domain;
using Shared.Domain.Enums;
using Site.Application.Contract.MenuService.Command;

namespace Site.Domain.MenuAgg
{
    public interface IMenuRepository : IGenericRepository<Menu,int>
    {
        Menu GetById(int id);
       Task< EditMenuCommandModel> GetForEdit(int id);
        Task<bool> ExistMainMenu(MenuStatus status);
       
    }
}
