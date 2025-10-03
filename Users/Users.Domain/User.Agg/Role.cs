using Shared.Domain;

namespace Users.Domain.User.Agg
{
    public class Role : BaseEntityCreate<int>
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


        public void AddPermission(int permissionId,int roleId)
        {
            if (RolePermissions.All(x => x.PermissionId != permissionId))
                RolePermissions.Add(new RolePermission(roleId, permissionId));
        }

 

        public void ClearPermissions()
        {
            RolePermissions.Clear();
        }
    }




}
