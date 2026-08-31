using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Contract.UserAddressService.Query;
using Users.Domain.User.Agg;
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

        public async Task<UserAddressForOrderQueryModel> GetByIdAsync(int addressId)
        {
           var Address= await _userAddressRepository.GetByIdAsync(addressId);
            return new()
            {
                CityId = Address.CityId,
                StateId = Address.StateId,
                AddressDetail = Address.AddressDetail,
                FullName = Address.FullName,
                NationalCode = Address.NationalCode,
                Phone = Address.Phone,
                PostalCode = Address.PostalCode,
            };
        }

        public async Task<UserAddressForOrderQueryModel> GetUserDefaultAddress(int UserId)
        {
            var defaultAddress = await _userAddressRepository.GetDefaultAddressAsync(UserId);
            return new()
            {
                CityId = defaultAddress.CityId,
                StateId = defaultAddress.StateId,
                AddressDetail = defaultAddress.AddressDetail,
                FullName = defaultAddress.FullName,
                NationalCode = defaultAddress.NationalCode,
                Phone = defaultAddress.Phone,
                PostalCode = defaultAddress.PostalCode,
            };
        }
    }
}
