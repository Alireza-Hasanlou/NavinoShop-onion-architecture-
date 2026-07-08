using Shared.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Contract.Cart
{
    public interface ICartCommands
    {
        Task<OperationResult> ClearCartAsync(int userId);
        Task<OperationResult> CreateAsync(int UserId, int ProductSellId, int Quantity);
        Task<OperationResult> DeleteAsync(int uesrId, int ProductId);
        Task<OperationResult> UpdateQuantityAsync(int userId, int productId, int change);
    }
}
