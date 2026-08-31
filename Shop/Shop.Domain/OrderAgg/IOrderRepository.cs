using Shared.Domain;
using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.OrderAgg
{
    public interface IOrderRepository : IGenericRepository<Order, int>
    {
        Task<Order> GetOpenOrderForUserAsync(int userId );
        Task<bool> SetOrderPaymentTypeAsync(int userId, OrderPayment orderPayment);
        Task<Order> GetOpenOrderForFinalizePaymentAsync(int userId);
    }
}
