using Discount.Domain.OrderDiscountAgg;
using Discount.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Insfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Infrastructure.Persistence.Repository
{
    internal class OrderDiscountsRepository:GenericRepository<OrderDiscount,int>, IOrderDiscountRepository
    {
        private readonly DiscountContext _discountContext;

        public OrderDiscountsRepository(DiscountContext context) : base(context)
        {
            _discountContext = context;
        }

        public async Task<bool> IsExistByCodeAsync(string code, int shopId)
        {
            return await _discountContext.OrderDiscounts.AnyAsync(x=>x.Code==code && x.ShopId==shopId);
        }
    }
}
