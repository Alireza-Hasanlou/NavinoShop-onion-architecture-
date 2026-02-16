using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Application.Contract.UserService.Query
{
    public interface IUserQueryService
    {
        Task<UserHeaderQueryModel> GetUserForHeader(int id);
        Task<EditUserByAdminDto> GetForEditByAdminAsync(int userId);
        Task<List<UserQueryModel>> GetUsersByIds(List<int> Ids);
        Task<AdminUserPaging> GetUsersForAdminAsync(int pageId, int take, string filter);
        Task<UserDetailDto> GetUserDetailForAdminAsync(int userId);
        Task<AdminUserPaging> GetDeletedUserForAdmin(int pageId, int take, string filter);
    }
}
