using Shared.Domain;
using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.OrderSellerAgg
{
    public interface IOrderSellerRepository : IGenericRepository<OrderSeller, int>
    {
        Task<int> ChangeOrderSellerStatusAsync(int sellerId, int orderId, OrderSellerStatus status);
    }
}
