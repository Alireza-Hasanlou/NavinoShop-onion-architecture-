using Utility.Shared.Domain;


namespace Users.Domain.User.Agg
{
    public class Permission : BaseEntity<int>
    {
        public string Title { get; private set; }
        public string? Description { get; private set; }

        public ICollection<RolePermission> RolePermissions { get; private set; }



        public Permission(string title, string? description)
        {
            Title = title;
            Description = description;
            RolePermissions = new List<RolePermission>();
        }

        public void Edit(string title, string? description)
        {
            Title = title;
            Description = description;
        }
    }




}
