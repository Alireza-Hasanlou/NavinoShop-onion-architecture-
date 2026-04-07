using Shared.Domain;
using Shop.Domain.ProductAgg;
using Shop.Domain.ProductCategoryAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Relations.ProductCategoryRel
{
    public class Product_Category_Rel : BaseEntity<int>
    {
        public Product_Category_Rel(int productId, int categoryId)
        {
            ProductId = productId;
            CategoryId = categoryId;
        }
        public Product_Category_Rel(int categoryId)
        {
            CategoryId = categoryId;
        }

        public int ProductId { get; private set; }
        public int CategoryId { get; private set; }

        public Product Product { get; private set; }
        public ProductCategory ProductCategory { get; private set; }


    }
}
