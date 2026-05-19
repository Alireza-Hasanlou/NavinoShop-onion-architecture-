using Shared.Domain;
using Shop.Application.Contract.Seller.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.SellerChangeRequestsAgg
{
    public interface ISellerChangeRequestsRepository : IGenericRepository<SellerChangeRequest, int>
    {
        Task <List<SellersChangeRequestQueryModel>> GetChangeRequests();
    }
}
