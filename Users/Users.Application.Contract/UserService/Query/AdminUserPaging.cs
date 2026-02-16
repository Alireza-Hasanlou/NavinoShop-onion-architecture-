using Shared;

namespace Users.Application.Contract.UserService.Query
{
    public class AdminUserPaging : BasePaging
    {
        public string Filter { get; set; }
        public List<UsersForAdminDto> Users { get; set; }
    }
}