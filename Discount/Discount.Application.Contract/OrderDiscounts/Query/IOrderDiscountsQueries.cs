using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Contract.OrderDiscounts.Query
{
    public interface IOrderDiscountsQueries
    {
        Task<List<OrderDiscountsQueryModel>> GeAllAsync(int ShopId, OrderDiscountType type);
        Task<List<OrderDiscountsQueryModel>> GeAllExpiredDiscountsAsync(int ShopId, OrderDiscountType type);

    }
}
