using Shared.Application;
using Shared.Domain;
using Users.Application.Contract.RoleService.Command;
using Users.Application.Contract.RoleService.Query;

namespace Users.Domain.User.Agg.IRepository
{
    public interface IRoleRepository : IGenericRepository<Role, int>
    {
        bool CheckPermission(int userId, int permissionId);
        Task<OperationResult> EditAsync(EditRoleCommand command, List<int> permissions);
        Task<List<PermissionQueryModel>> GetAllPermission();
        Task<List<RoleQueryModel>> GetAllRoles();
        Task<EditRoleQueryModel> GetForEdit(int id);
        Task<UsersRoleQuryModel> GetUsersInRole(int roleId);
    }
}
