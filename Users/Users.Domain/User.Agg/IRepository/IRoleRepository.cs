using Users.Application.Contract.RoleService.Query;
using Utility.Shared.Domain;

namespace Users.Domain.User.Agg.IRepository
{
    public interface IRoleRepository : IGenericRepository<Role, int>
    {
        bool CheckPermission(int userId, int permissionId);
        Task<List<PermissionQueryModel>> GetAllPermission();
        Task<List<RoleQueryModel>> GetAllRoles();
        Task<EditRoleDto> GetForEdit(int id);
        Task<UsersRoleQuryModel> GetUsersInRole(int roleId);
    }
}
