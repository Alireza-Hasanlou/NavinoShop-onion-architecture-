using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Contract.RoleService.Query;
using Users.Domain.User.Agg.IRepository;

namespace Users.Query.Service
{
    internal class RoleQueryService : IRoleQueryService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleQueryService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public bool CheckPermission(int userId, int permissionId)
        {
           return _roleRepository.CheckPermission(userId, permissionId);
        }

        public EditRoleDto GetForEdit(int id)
        {
            throw new NotImplementedException();
        }

        public List<RolePermissionQueryModel> GetPermissionsForRole(int id)
        {
            throw new NotImplementedException();
        }
    }
}
