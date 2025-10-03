using Shared.Domain;

namespace Users.Domain.User.Agg
{
    public class UserRole : BaseEntity<int>
    {
        public int UserId { get; private set; }
        public int RoleId { get; private set; }

        public User User { get; private set; }
        public Role Role { get; private set; }


        public UserRole(int userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }




}
