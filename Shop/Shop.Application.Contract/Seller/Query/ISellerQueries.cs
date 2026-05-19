using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Contract.Seller.Query
{
    public interface ISellerQueries
    {
        Task<int> GetSellerUserIdAsync(int sellerId);
        Task<bool> IsSellerForUser(int userId, int sellerId);
        Task<RequestDetailQueryModel> GetSellerChangeRequestDeatail(int Id);
        Task<List<SellersChangeRequestQueryModel> >GetChangeRequests();
        Task<List<UsersShopQueryModel>> GetUsersShopAsync(int userId);
    }
}
