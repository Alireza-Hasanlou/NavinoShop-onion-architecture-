using Shared.Domain;

namespace Users.Domain.User.Agg
{
    public class RolePermission : BaseEntity<int>
    {
        public int RoleId { get; private set; }
        public int PermissionId { get; private set; }

        public Role Role { get; private set; }
        public Permission Permission { get; private set; }



        public RolePermission(int roleId, int permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }
    }




}
