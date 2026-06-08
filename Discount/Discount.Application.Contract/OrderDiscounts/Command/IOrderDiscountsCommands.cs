using Shared.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Contract.OrderDiscounts.Command
{
    public interface IOrderDiscountsCommands
    {
        Task<OperationResult> CreateOrderDiscountAsync(UpsertOrderDiscountCommandModel commandModel);
        Task<OperationResult> EditOrderDiscountAsync(UpsertOrderDiscountCommandModel commandModel);
        Task<OperationResult> DeleteAsync(int Id);
        Task<UpsertOrderDiscountCommandModel> GetForEditAsync(int id);
    }
}
