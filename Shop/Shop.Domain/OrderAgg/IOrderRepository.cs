using Shared.Application;
using Shared.Domain;
using Shared.Domain.Enums;
using Shop.Application.Contract.Order.Query;
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
        Task<Order> GetForEditOrderSellerStatusAsync(int orderId);
        Task<FactorforordercancellationQueryModel> GetFactorforordercancellationAsync(int orderId , int SellerId);
        Task<OperationResult> CancellOrderByUserAsync(int orderId);
        Task<OperationResult> CancellOrderByAdminAsync(int orderId);
    }
}
