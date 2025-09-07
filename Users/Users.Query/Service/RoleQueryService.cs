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

        public async Task<EditRoleDto> GetForEdit(int id)
        {
           return await _roleRepository.GetForEdit(id);
        }

        public bool CheckPermission(int userId, int permissionId)
        {
            return _roleRepository.CheckPermission(userId, permissionId);
        }

        public async Task<List<RoleQueryModel>> GetAllRoles()
        {
           return await _roleRepository.GetAllRoles();
        }

        public async Task<UsersRoleQuryModel> GetUsersInRole(int roleId)
        {
         return await _roleRepository.GetUsersInRole(roleId);   
        }

        public async Task<List<PermissionQueryModel>> GetAllPermission()
        {
           return await _roleRepository.GetAllPermission();
        }
    }
}
