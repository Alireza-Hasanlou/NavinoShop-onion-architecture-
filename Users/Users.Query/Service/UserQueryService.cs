using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Contract.UserService.Query;
using Users.Domain.User.Agg.IRepository;

namespace Users.Query.Service
{
    internal class UserQueryService : IUserQueryService
    {
        private readonly IUserRepository _userRepository;

        public UserQueryService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserQueryModel>> GetUsersByIds(List<int> Ids)
        {
            return await _userRepository.GetUsersByIds(Ids);
        }

        public Task<EditUserByAdminDto> GetForEditByAdminAsync(int userId)
        {
            throw new NotImplementedException();
        }


    }
}
