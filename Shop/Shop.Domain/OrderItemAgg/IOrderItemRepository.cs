using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.OrderItemAgg
{
    public interface IOrderItemRepository:IGenericRepository<OrderItem,int>
    {
    }
}
