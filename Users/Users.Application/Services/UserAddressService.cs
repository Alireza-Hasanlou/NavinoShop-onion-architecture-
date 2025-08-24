using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Contract.UserAddressService.Command;
using Users.Domain.User.Agg;
using Users.Domain.User.Agg.IRepository;
using Utility.Shared.Application;

namespace Users.Application.Services
{
    internal class UserAddressService : IUserAddressCommandService
    {
        private readonly IUserAddressRepository _userAddressRepository;

        public UserAddressService(IUserAddressRepository userAddressRepository)
        {
            _userAddressRepository = userAddressRepository;
        }

        public async Task<OperationResult> CreateAsync(CreateUserAddressCommand command, int userId)
        {
            var userAddress = new UserAddress(command.StateId, command.CityId, command.AddressDetail,
                command.PostalCode, command.Phone, command.FullName, command.NationalCode, command.UserId);
            var result = await _userAddressRepository.CreateAsync(userAddress);
            if (result.Success)
                return new(true);
            return new(false, ValidationMessages.SystemErrorMessage, "UserAddress");
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var address = await _userAddressRepository.GetByIdAsync(id);
            var result = await _userAddressRepository.DeleteAsync(address);

            if (result.Success)
                return new(true);
            return new(false, ValidationMessages.SystemErrorMessage, "UserAddress");
        }
    }
}
