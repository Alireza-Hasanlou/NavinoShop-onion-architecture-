using Microsoft.EntityFrameworkCore;
using Shared.Insfrastructure;
using Shop.Domain.Relations.ProductCategoryRel;
using Shop.Infrastracture.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastracture.Persistence.Repository
{
    internal class Product_Category_Repository : GenericRepository<Product_Category_Rel, int>, IProduct_Category_Repository
    {
        public Product_Category_Repository(ShopContext context) : base(context)
        {
        }
    }
}
