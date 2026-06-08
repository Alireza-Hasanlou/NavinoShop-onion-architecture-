using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Domain.ProductDiscountAgg
{
    public interface IProductDiscountRepository : IGenericRepository<ProductDiscount, int>
    {
        Task<ProductDiscount> GetByProductSellIdAsync(int productId, int productSellId);
        Task<List<ProductDiscount>> GetProductsDiscountAsync();

    }
}
