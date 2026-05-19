
using Microsoft.EntityFrameworkCore;
using Shop.Application.Contract.Seller.Command;
using Shop.Application.Contract.Seller.Query;
using Shop.Domain.SellerAgg;
using Shop.Domain.SellerChangeRequestsAgg;


namespace Shop.Query.Queries
{
    internal class SellerQueries : ISellerQueries
    {
        private readonly ISellerRepository _sellerRepository;
        private readonly ISellerChangeRequestsRepository _changeRequestsRepository;

        public SellerQueries(ISellerRepository sellerRepository, ISellerChangeRequestsRepository changeRequestsRepository)
        {
            _sellerRepository = sellerRepository;
            _changeRequestsRepository = changeRequestsRepository;
        }

        public async Task<List<SellersChangeRequestQueryModel>> GetChangeRequests()
        {
           return await _changeRequestsRepository.GetChangeRequests();
        }

        public async Task<RequestDetailQueryModel> GetSellerChangeRequestDeatail(int Id)
        {
            var request = await _changeRequestsRepository.GetByIdAsync(Id);
            if (request == null)
                return null;

            return new RequestDetailQueryModel
            {
                Id=request.Id,
                SellerId = request.Id,
                Address = request.Address,
                Description = request.Description,
                Email = request.Email,
                GoogleMapUrl = request.GoogleMapUrl,
                AvatarImageName = request.Avatar,
                Instagram = request.Instagram,
                Phone1 = request.Phone1,
                Phone2 = request.Phone2,
                Telegram = request.Telegram,
                Title = request.Title,
                WhatsApp = request.WhatsApp,
                CoverImageName = request.CoverImage,
                Status=request.status
            };
        }

        public async Task<int> GetSellerUserIdAsync(int sellerId)
        {
            return await _sellerRepository.GetSellerUserIdAsync(sellerId);
        }

        public async Task<List<UsersShopQueryModel>> GetUsersShopAsync(int userId)
        {
            return await _sellerRepository.GetAllBy(u => u.UserId == userId)
                .Select(u => new UsersShopQueryModel
                {
                    Id = u.Id,
                    Title = u.Title,
                }).ToListAsync();
        }

        public async Task<bool> IsSellerForUser(int userId, int sellerId)
        {
            return await _sellerRepository.ExistByAsync(x => x.UserId == userId && x.Id == sellerId);
        }
    }
}
