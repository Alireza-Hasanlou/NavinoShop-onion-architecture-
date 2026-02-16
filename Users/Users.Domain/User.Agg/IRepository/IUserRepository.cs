using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Contract.UserService.Query;

namespace Users.Domain.User.Agg.IRepository
{
    public interface IUserRepository : IGenericRepository<User, int>
    {
        Task<List<UserQueryModel>> GetUsersByIds(List<int> Ids);
        Task<User> GetByMobile(string mobile);
        Task<EditUserByUserDto> GetForEditByUserAsync(int userId);
        Task<UserHeaderQueryModel> GetUserForHeader(int id);
        Task<UserDetailDto> GetUserDetailAsync(int userId);
    }
}
