using Discount.Domain.ProductDiscountAgg;
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
    internal class ProductDiscountRepository : GenericRepository<ProductDiscount, int>, IProductDiscountRepository
    {
        private readonly DiscountContext _discountContext;

        public ProductDiscountRepository(DiscountContext discountContext) : base(discountContext)
        {
            _discountContext = discountContext;
        }



        public async Task<ProductDiscount> GetByProductSellIdAsync(int productId, int productSellId)
        {
            return await _discountContext.ProductDiscounts.SingleOrDefaultAsync(x => x.ProductId == productId && x.ProductSellId == productSellId);
        }

        public async Task<List<ProductDiscount>> GetProductsDiscountAsync()
        {
            return await _discountContext.ProductDiscounts.ToListAsync();
        }

       
    }
}
