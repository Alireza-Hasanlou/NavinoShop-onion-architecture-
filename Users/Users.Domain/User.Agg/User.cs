using Shared.Domain;
using Shared.Domain.Enums;
using System;



namespace Users.Domain.User.Agg
{
    public class User : BaseEntityCreate<int>
    {
        public string FullName { get; private set; }
        public string Mobile { get; private set; }
        public string? Email { get; private set; }
        public string Password { get; private set; }
        public string Avatar { get; private set; }
        public bool Active { get; private set; }
        public bool IsDelete { get; private set; }
        public int WalletId { get; private set; }
        public Gender UserGender { get; private set; }
        public ICollection<UserAddress> Addresses { get; private set; }
        public ICollection<UserRole> UserRoles { get; private set; }


        protected User()
        {

        }
        public User(string fullName, string mobile, string? email,
                    string password, string avatar,
                    bool active, bool isDelete, Gender gender)
        {
            FullName = fullName;
            Mobile = mobile;
            Email = email;
            Password = password;
            Avatar = avatar;
            Active = active;
            IsDelete = isDelete;
            UserGender = gender;
            Addresses = new List<UserAddress>();
            UserRoles = new List<UserRole>();


        }

        public void Edit(string fullName, string mobile, string email,
                               string password, string avatar, Gender gender, List<int> RoleIds)
        {
            FullName = fullName;
            Mobile = mobile;
            Email = email;
            Avatar = avatar;
            UserGender = gender;
            Password = password;
            if (RoleIds.Count() > 0)
            {
                ClearRole();
                foreach (var roleid in RoleIds)
                {
                    AddRole(roleid);
                }
            }
        }

        public void ActivationChange()
        {
            if (Active)
                Active = false;

            Active = true;
        }

        public void DeleteChange()
        {
            if (IsDelete)
            {
                IsDelete = false;
            }
            else
            {
                IsDelete = true;
            }


        }

        public static User Register(string mobile, string password)
        {
            return new User("", mobile, "", password, "DefaultAvatar.png", false, false, Gender.نامشخص);
        }

        public void AddRole(int roleId)
        {
            if (UserRoles.All(x => x.RoleId != roleId))
                UserRoles.Add(new UserRole(Id, roleId));
        }
        public void ClearRole()
        {
            UserRoles.Clear();
        }
        public void RemoveRole(Role role)
        {
            var userRole = UserRoles.FirstOrDefault(x => x.RoleId == role.Id);
            if (userRole != null)
                UserRoles.Remove(userRole);
        }
        public void ChangePassword(string pass)
        {
            Password = pass;
        }
    }

}
