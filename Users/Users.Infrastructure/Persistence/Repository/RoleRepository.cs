using Microsoft.EntityFrameworkCore;
using Shared.Application;
using Shared.Application.Auth;
using Shared.Application.Validations;
using Shared.Insfrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.Application.Contract.RoleService.Command;
using Users.Application.Contract.RoleService.Query;
using Users.Domain.User.Agg;
using Users.Domain.User.Agg.IRepository;
using Users.Infrastructure.Persistence.Context;

namespace Users.Infrastructure.Persistence.Repository
{
    public class RoleRepository : GenericRepository<Role, int>, IRoleRepository
    {
        private readonly UserContext _userContext;

        public RoleRepository(UserContext userContext) : base(userContext)
        {
            _userContext = userContext;
        }


        public bool CheckPermission(int userId, int permissionId)
        {
            return _userContext.UserRoles
                .Any(ur => ur.UserId == userId &&
                           ur.Role.RolePermissions.Any(rp => rp.PermissionId == permissionId));
        }

        public async Task<OperationResult> EditAsync(EditRoleCommand command, List<int> permissions)
        {
            var role = await _userContext.Roles.Where(i => i.Id == command.Id).Include(r => r.RolePermissions).SingleAsync();

            if (await ExistByAsync(t => t.Title == command.Title.Trim().ToLower() && role.Id != command.Id))
                return new(false, ValidationMessages.DuplicatedMessage, "Role");

            role.Edit(command.Title.Trim().ToLower());

            if (permissions.Count > 0)
            {
                role.ClearPermissions();


                foreach (var permission in permissions)
                {
                    role.AddPermission(permission, role.Id);
                }

            }

            var result = await SaveAsync();
            if (result)
                return new(true);

            return new(false, ValidationMessages.SystemErrorMessage, "Role");
        }

        public async Task<List<PermissionQueryModel>> GetAllPermission()
        {
            return await _userContext.Permissions.Select(item => new PermissionQueryModel
            {
                Id = item.Id,
                Title = item.Title,
            }).ToListAsync();
        }

        public async Task<List<RoleQueryModel>> GetAllRoles()
        {
            return await _userContext.Roles
                .Where(r => r.Id != 1) // exclude SiteManager
                .Select(r => new RoleQueryModel
                {
                    RoleId = r.Id,
                    RoleTitle = r.Title,
                    RoleCreateDate = r.CreateDate.ToPersainDate()
                })
                .ToListAsync();
        }


        public async Task<EditRoleQueryModel> GetForEdit(int id)
        {


            var role = await _userContext.Roles.Where(i => i.Id == id).Select(item => new EditRoleQueryModel
            {
                Id = item.Id,
                Title = item.Title,
                permissions = item.RolePermissions.Select(i => i.PermissionId).ToList()
            }).SingleOrDefaultAsync();

            if (role == null)
                return new();

            return role;


        }


        public async Task<UsersRoleQuryModel> GetUsersInRole(int roleId)
        {


          var usersInRole = await  _userContext.Roles.Where(i => i.Id == roleId).Include(u => u.UserRoles).ThenInclude(u => u.User).Select(role => new UsersRoleQuryModel
            {
                RoleTitle = role.Title,
                Users = role.UserRoles.Select(item => new UsersQueryModel
                {
                    Userid = item.User.Id,
                    UserFullName = item.User.FullName,
                    Mobile = item.User.Mobile,
                    UserAvatar = item.User.Avatar
                }).ToList()
            }).SingleOrDefaultAsync();

            if(usersInRole == null) return new();   
            
            return usersInRole;
            
        }
    }
}
