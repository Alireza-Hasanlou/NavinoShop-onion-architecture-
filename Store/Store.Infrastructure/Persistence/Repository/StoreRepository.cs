using Microsoft.EntityFrameworkCore;
using Shared.Insfrastructure;
using Store.Domain.StoreAgg;
using Store.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Infrastructure.Persistence.Repository
{
    internal class StoreRepository : GenericRepository<Store.Domain.StoreAgg.Store, int>, IStoreRepository
    {
        private readonly StoreContext _storeContext;

        public StoreRepository(StoreContext context) : base(context)
        {
            _storeContext = context;
        }


    }
}
