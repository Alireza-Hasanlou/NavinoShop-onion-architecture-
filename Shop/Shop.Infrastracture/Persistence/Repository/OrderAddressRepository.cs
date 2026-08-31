using Shared.Insfrastructure;
using Shop.Domain.OrderAddressAgg;
using Shop.Infrastracture.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastracture.Persistence.Repository
{
    internal class OrderAddressRepository : GenericRepository<OrderAddress, int>, IOrderAddressRepository
    {
        private readonly ShopContext _shopContext;

        public OrderAddressRepository(ShopContext shopContext) : base(shopContext)
        {
            _shopContext = shopContext;
        }
    }
}
