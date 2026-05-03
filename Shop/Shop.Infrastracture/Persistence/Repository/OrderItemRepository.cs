using Microsoft.EntityFrameworkCore;
using Shared.Insfrastructure;
using Shop.Domain.OrderItemAgg;
using Shop.Infrastracture.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastracture.Persistence.Repository
{
    internal class OrderItemRepository : GenericRepository<OrderItem, int>, IOrderItemRepository
    {
        private readonly ShopContext _shopContext;
        public OrderItemRepository(ShopContext context) : base(context)
        {

        }
    }
}
