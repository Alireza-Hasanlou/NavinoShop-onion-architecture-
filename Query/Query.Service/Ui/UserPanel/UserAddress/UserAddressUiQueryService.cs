using Microsoft.EntityFrameworkCore;
using PostModule.Domain.Services;
using Query.Contract.UI.UserPanel.UserAddress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.User.Agg.IRepository;

namespace Query.Service.Ui.UserPanel.UserAddress
{
    internal class UserAddressUiQueryService:IUserAddressUiQueryService
    {
        private readonly IUserAddressRepository _userAddressRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;

        public UserAddressUiQueryService(IUserAddressRepository userAddressRepository, 
            IStateRepository stateRepository, ICityRepository cityRepository)
        {
            _userAddressRepository = userAddressRepository;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
        }

        public async Task<List<UserAddressQueryModel>> GetUserAddressesAsync(int UserId)
        {


            var addresses = await _userAddressRepository.GetAllBy(i => i.UserId == UserId)
                    .OrderByDescending(i => i.Id)
                     .Select(u => new UserAddressQueryModel
                     {
                         Id = u.Id,
                         FullName = u.FullName,
                         AddressDetail = u.AddressDetail,
                         NationalCode = u.NationalCode,
                         PostalCode = u.PostalCode,
                         Phone = u.Phone,
                         CityId = u.CityId,
                         StateId = u.StateId,
                         State = "",
                         City = ""

                     }).ToListAsync();
            foreach (var address in addresses)
            {
                address.State = await _stateRepository.GetStateTitle(address.StateId);
                address.City = await _cityRepository.GetCityTitle(address.CityId);
            }
            return addresses;
        }
    }
}
