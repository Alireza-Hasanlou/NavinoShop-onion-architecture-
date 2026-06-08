using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Contract.ProductSell.Query
{
    public interface IProductSellQueries
    {
        Task<bool> ProductSellHaveAmount(int Id);
    }
}
