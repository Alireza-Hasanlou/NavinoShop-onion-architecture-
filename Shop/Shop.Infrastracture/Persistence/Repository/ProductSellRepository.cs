using Microsoft.EntityFrameworkCore;
using Shared.Insfrastructure;
using Shop.Domain.ProductSellAgg;
using Shop.Infrastracture.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastracture.Persistence.Repository
{
    internal class ProductSellRepository : GenericRepository<ProductSell, int>, IProductSellRepository
    {
        public ProductSellRepository(ShopContext context) : base(context)
        {
        }
    }
}
