using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Domain.OrderDiscountAgg
{
    public interface IOrderDiscountRepository : IGenericRepository<OrderDiscount, int>
    {

    }
}