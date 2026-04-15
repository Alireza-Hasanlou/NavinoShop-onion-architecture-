using Microsoft.EntityFrameworkCore;
using PostModule.Domain.Services;
using Query.Contract.Admin.Seller;
using Shared.Application;
using Shop.Domain.SellerAgg;
using Users.Domain.User.Agg.IRepository;

namespace Query.Service.Admin.Seller
{
    internal class AdminSellerQueryService : IAdminSellerQueryService
    {
        private readonly ISellerRepository _sellerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;
        public AdminSellerQueryService(ISellerRepository sellerRepository, IUserRepository userRepository,
            IStateRepository stateRepository, ICityRepository cityRepository)
        {
            _sellerRepository = sellerRepository;
            _userRepository = userRepository;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
        }


        public async Task<List<SellersRequrstAdminQueryModel>> GetAllSalesRequrstForAdmin()
        {
            var sellers = await _sellerRepository.GetAll()
                .Select(s => new SellersRequrstAdminQueryModel
                {
                    Id = s.Id,
                    ImageName = s.ImageName,
                    WhyRejected = s.WhyRejected,
                    LicenseImage = s.LicenseImage,
                    Phone1 = s.Phone1,
                    UserId = s.UserId,
                    Title = s.Title,
                    StateId = s.StateId,
                    CityId = s.CityId,
                    Status = s.Status,
                    RequestDate = s.CreateDate,
                    CityName = "",
                    UserName = ""
                }).ToListAsync();

            var userIds = sellers.Select(s => s.UserId).ToList();
            var users = await _userRepository.GetUsersByIds(userIds);
            foreach (var item in sellers)
            {

                var state = await _stateRepository.GetByIdAsync(item.StateId);
                var city = await _cityRepository.GetByIdAsync(item.CityId);
                item.CityName = $"{state.Title}_{city.Title}";

                var user = users.FirstOrDefault(u => u.Id == item.UserId);
                item.UserName = user?.FullName ?? user?.Mobile;
            }

            return sellers;

        }

        public async Task<SellerRequestDetailQueryModel> GetSellerRequestDetail(int Id)
        {
            var seller = await _sellerRepository.GetByIdAsync(Id);
            if (seller == null)
                return null;
            var state = await _stateRepository.GetByIdAsync(seller.StateId);
            var city = await _cityRepository.GetByIdAsync(seller.CityId);
            var user = await _userRepository.GetByIdAsync(seller.UserId);
            return new SellerRequestDetailQueryModel
            {
                Id = seller.Id,
                ShopImageName = FileDirectories.SellerImageDirectory500 + seller.ImageName,
                LicenseImage = FileDirectories.SellerImageDirectory500 + seller.LicenseImage,
                Phone1 = seller.Phone1,
                Title = seller.Title,
                RequestDate = seller.CreateDate.ToPersainDate(),
                CityName = $"{state.Title}_{city.Title}",
                UserName = user.FullName,
                UserImaneName = FileDirectories.UserImageDirectory100 + user.Avatar,
                Address = seller.Address,
                Email = seller.Email,
                ImageAlt = seller.ImageAlt,
                Instagram = seller.Instagram,
                MapUrl = seller.MapUrl,
                Phone2 = seller.Phone2,
                Status = seller.Status,
                Telegram = seller.Telegram,
                Whatsup = seller.Whatsup
            };


        }
    }
}
