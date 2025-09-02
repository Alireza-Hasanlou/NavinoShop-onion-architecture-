using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Application.Contract.RoleService.Query
{
    public interface IRoleQueryService
    {
        EditRoleDto GetForEdit(int id);
        List<RolePermissionQueryModel> GetPermissionsForRole(int id);
        bool CheckPermission(int userId, int permissionId);
    }
}
