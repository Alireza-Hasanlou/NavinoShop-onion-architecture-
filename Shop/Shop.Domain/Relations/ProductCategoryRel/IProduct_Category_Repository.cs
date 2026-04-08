using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Relations.ProductCategoryRel
{
    public interface IProduct_Category_Repository : IGenericRepository<Product_Category_Rel, int>
    {
    }
}
