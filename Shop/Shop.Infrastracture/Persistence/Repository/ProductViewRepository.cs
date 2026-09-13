using Microsoft.EntityFrameworkCore;
using Shared.Insfrastructure;
using Shop.Domain.ProductViewAgg;
using Shop.Infrastracture.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastracture.Persistence.Repository
{
    internal class ProductViewRepository: GenericRepository<ProductView,long>, IProductViewRepository
    {
        private readonly ShopContext _context;

        public ProductViewRepository(ShopContext context) : base(context)
        {
            _context = context;
        }
    }
}
