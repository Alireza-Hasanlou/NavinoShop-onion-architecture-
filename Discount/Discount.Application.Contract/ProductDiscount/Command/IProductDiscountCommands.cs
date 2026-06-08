using Shared.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Contract.ProductDiscount.Command
{
    public interface IProductDiscountCommands
    {
        Task<OperationResult> UpSertAsync(UpsertProductDiscountCommandModel command);
        Task<UpsertProductDiscountCommandModel> GetForUpsertAsync(int ProductId,int ProductSellId);
        Task<OperationResult> DeleteAsync(int Id);
    }
}
