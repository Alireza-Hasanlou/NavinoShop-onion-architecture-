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

    }
}
