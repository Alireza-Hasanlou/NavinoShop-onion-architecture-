using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Application.Contract.RoleService.Query
{
    public interface IRoleQueryService
    {
        Task<EditRoleDto> GetForEdit(int id);
        Task<List<PermissionQueryModel>> GetAllPermission();                                                                         
        bool CheckPermission(int userId, int permissionId);
        Task<List<RoleQueryModel>> GetAllRoles();
        Task<UsersRoleQuryModel> GetUsersInRole(int roleId);
    }
}
