using Shared.Application;
using Shared.Application.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Contract.UserAddressService.Command;
using Users.Application.Contract.UserAddressService.Query;
using Users.Domain.User.Agg;
using Users.Domain.User.Agg.IRepository;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            if (await _userAddressRepository.ExistByAsync(p => p.PostalCode == command.PostalCode))
                return new(false, ValidationMessages.DuplicatedMessage);
            if( await _userAddressRepository.GetUserAddressCount()>=10)
                return new(false,"تعداد آدرس ها بیش از حد مجاز است");
            var userAddress = new UserAddress(command.StateId, command.CityId, command.AddressDetail,
                command.PostalCode, command.Phone, command.FullName, command.NationalCode, userId);
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

        public async Task<OperationResult> EditAsync(UserAddressDto command )
        {
            var address = await _userAddressRepository.GetByIdAsync(command.Id);
            address.Edit(command.StateId, command.CityId, command.AddressDetail, command.PostalCode, command.Phone, command.FullName, command.NationalCode);
            if (await _userAddressRepository.SaveAsync())
                return new(true);
            return new(false, ValidationMessages.SystemErrorMessage);

        }

        public async Task<UserAddressDto> GetAddressForEditAsync(int id)
        {
            return await _userAddressRepository.GetAddressForEditAsync(id);
        }
    }
}
