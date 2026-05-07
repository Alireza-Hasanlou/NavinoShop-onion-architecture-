
using Microsoft.EntityFrameworkCore;
using Shop.Application.Contract.Seller.Query;
using Shop.Domain.SellerAgg;


namespace Shop.Query.Queries
{
    internal class SellerQueries : ISellerQueries
    {
        private readonly ISellerRepository _sellerRepository;

        public SellerQueries(ISellerRepository sellerRepository)
        {
            _sellerRepository = sellerRepository;
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
