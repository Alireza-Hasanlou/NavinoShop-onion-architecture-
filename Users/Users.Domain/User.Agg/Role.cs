using Utility.Shared.Domain;


namespace Users.Domain.User.Agg
{
    public class Role : BaseEntity<int>
    {
        public string Title { get; private set; }

        public ICollection<UserRole> UserRoles { get; private set; }
        public ICollection<RolePermission> RolePermissions { get; private set; }



        public Role(string title)
        {
            Title = title;
            UserRoles = new List<UserRole>();
            RolePermissions = new List<RolePermission>();
        }

        public void Edit(string title)
        {
            Title = title;
        }


        public void AddPermission(Permission permission)
        {
            if (RolePermissions.All(x => x.PermissionId != permission.Id))
                RolePermissions.Add(new RolePermission(Id, permission.Id));
        }

        public void RemovePermission(Permission permission)
        {
            var rolePermission = RolePermissions.FirstOrDefault(x => x.PermissionId == permission.Id);
            if (rolePermission != null)
                RolePermissions.Remove(rolePermission);
        }
    }




}
