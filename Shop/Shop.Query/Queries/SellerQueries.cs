
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
            return await  _sellerRepository.GetSellerUserIdAsync(sellerId);
        }
    }
}
