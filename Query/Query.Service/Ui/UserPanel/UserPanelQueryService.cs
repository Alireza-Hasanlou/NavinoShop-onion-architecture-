using Microsoft.EntityFrameworkCore;
using PostModule.Domain.Services;
using Query.Contract.UI.UserPanel;
using Shared.Application;
using Users.Domain.User.Agg.IRepository;

namespace Query.Service.Ui.UserPanel
{
    internal class UserPanelQueryService : IUserPanelQueryService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserAddressRepository _userAddressRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;

        public UserPanelQueryService(IUserRepository userRepository, IUserAddressRepository userAddressRepository,
            IStateRepository stateRepository, ICityRepository cityRepository)
        {
            _userRepository = userRepository;
            _userAddressRepository = userAddressRepository;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
        }

        public async Task<UserPanelQueryModel> GetUserInfoForPanel(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return new UserPanelQueryModel()
            {
                Id = user.Id,
                Name = user.FullName,
                Email = user.Email,
                PhoneNumber = user.Mobile,
                RegisterDate = user.CreateDate.ToPersainDate(),
                Avatar = user.Avatar,
                Gender = user.UserGender,
                TransactionCount = 0,
                TransactionSum = 0,

            };

        }

        public async Task<List<UserAddressQueryModel>> GetUserAddressesAsync(int UserId)
        {

            var addresses = await _userAddressRepository.GetAllBy(i => i.UserId == UserId)
                .OrderByDescending(i => i.Id)
                 .Select(u => new UserAddressQueryModel
                 {
                     Id=u.Id,
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
