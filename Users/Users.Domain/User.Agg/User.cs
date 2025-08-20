using Shared.Domain.Enums;
using System;
using Utility.Shared.Domain;


namespace Users.Domain.User.Agg
{
    public class User : BaseEntity<int>
    {
        public string FullName { get; private set; }
        public string Mobile { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Avatar { get; private set; }
        public string Key { get; private set; }
        public bool Active { get; private set; }
        public bool Delete { get; private set; }
        public Gender UserGender { get; private set; }
        public ICollection<UserAddress> Addresses { get; private set; }
        public ICollection<UserRole> UserRoles { get; private set; }


        public User(string fullName, string mobile, string email,
                    string password, string avatar, string key,
                    bool active, bool delete, Gender gender)
        {
            FullName = fullName;
            Mobile = mobile;
            Email = email;
            Password = password;
            Avatar = avatar;
            Key = key;
            Active = active;
            Delete = delete;
            UserGender = gender;
            Addresses = new List<UserAddress>();
            UserRoles = new List<UserRole>();
        }

        public void Edit(string fullName, string mobile, string email,
                               string password, string avatar, Gender gender)
        {
            FullName = fullName;
            Mobile = mobile;
            Email = email;
            Avatar = avatar;
            UserGender = gender;
            if (!string.IsNullOrEmpty(password))
                Password = password;
        }

        public void ActivationChange()
        {
            if (Active)
                Active = false;

            Active = true;
        }

        public void DeleteChange()
        {
            if (Delete)
                Delete = false;

            Delete = true;
        }

        public User Register(string mobile, string password, string key)
        {
            return new User("", mobile, "", password, "Defult.png", key, false, false, Gender.نامشخص);
        }

        public void AddRole(Role role)
        {
            if (UserRoles.All(x => x.RoleId != role.Id))
                UserRoles.Add(new UserRole(Id, role.Id));
        }

        public void RemoveRole(Role role)
        {
            var userRole = UserRoles.FirstOrDefault(x => x.RoleId == role.Id);
            if (userRole != null)
                UserRoles.Remove(userRole);
        }
    }

}
