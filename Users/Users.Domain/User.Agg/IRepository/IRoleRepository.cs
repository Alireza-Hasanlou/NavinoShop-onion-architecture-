using Utility.Shared.Domain;

namespace Users.Domain.User.Agg.IRepository
{
    public interface IRoleRepository : IGenericRepository<Role, int>
    {
       bool CheckPermission(int userId, int permissionId);
    }
}
