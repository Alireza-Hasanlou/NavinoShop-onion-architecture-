using Microsoft.EntityFrameworkCore;
using Users.Domain.User.Agg;
using Users.Domain.User.Agg.IRepository;
using Users.Infrastructure.Persistence.Context;
using Utility.Shared.Insfrastructure;

namespace Users.Infrastructure.Persistence.Repository
{
    internal class RoleRepository : GenericRepository<Role, int>, IRoleRepository
    {
        private readonly UserContext _userContext;

        public RoleRepository(UserContext userContext) : base(userContext)
        {
            _userContext = userContext;
        }

        public  bool CheckPermission(int userId, int permissionId)
        {
            return  _userContext.UserRoles
                .Any(ur => ur.UserId == userId &&
                                ur.Role.RolePermissions.Any(rp => rp.PermissionId == permissionId));

        }
    }
}
