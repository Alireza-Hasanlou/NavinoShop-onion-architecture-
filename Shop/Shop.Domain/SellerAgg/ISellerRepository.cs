using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.SellerAgg
{
    public interface ISellerRepository:IGenericRepository<Seller,int>
    {
    }
}
