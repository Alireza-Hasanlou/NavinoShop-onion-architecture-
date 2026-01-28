using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Contract.UserAddressService.Query;
using Users.Domain.User.Agg.IRepository;

namespace Users.Query.Service
{
    internal class UserAddressQueryService : IUserAddressQueryService
    {
        private readonly IUserAddressRepository _userAddressRepository;

        public UserAddressQueryService(IUserAddressRepository userAddressRepository)
        {
            _userAddressRepository = userAddressRepository;
        }

        public Task<UserAddressDto> GetAddressForAddToFactorAsync(int id)
        {
            throw new NotImplementedException();
        }

       
        public Task<bool> IsAddressForUserAsync(int id, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
