using Microsoft.EntityFrameworkCore;
using Shared.Insfrastructure;
using Store.Domain.StoreProductAgg;
using Store.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Infrastructure.Persistence.Repository
{
    internal class StoreProductRepository : GenericRepository<StoreProduct, int>, IStoreProductRepository
    {
        private readonly StoreContext _storeContext;

        public StoreProductRepository(StoreContext context) : base(context)
        {
            _storeContext = context;
        }

  
    }
}