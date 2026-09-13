using Shared.Domain;
using Store.Application.Contract.StoreProduct.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.StoreProductAgg
{
    public interface IStoreProductRepository : IGenericRepository<StoreProduct, int>
    {
 
    }
}
