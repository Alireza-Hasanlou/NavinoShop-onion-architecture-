using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.OrderAddressAgg
{
    public interface IOrderAddressRepository:IGenericRepository<OrderAddress,int>
    {
    }
}
