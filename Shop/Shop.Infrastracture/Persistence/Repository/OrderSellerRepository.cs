using Shared.Insfrastructure;
using Shop.Domain.OrderSellerAgg;
using Shop.Infrastracture.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastracture.Persistence.Repository
{
    internal class OrderSellerRepository : GenericRepository<OrderSeller, int>, IOrderSellerRepository
    {
        private readonly ShopContext _shopContext;

        public OrderSellerRepository(ShopContext shopContext) : base(shopContext)
        {
            _shopContext = shopContext;
        }
    }
}
