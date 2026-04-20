using Shared.Domain;
using Shop.Domain.ProductAgg;
using Shop.Domain.Relations.ProductCategoryRel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ProductCategoryAgg
{
    public class ProductCategory : BaseEntityCreateUpdateActive<int>
    {
        public ProductCategory(string title, string slug, int parentId)
        {
            Title = title;
            Slug = slug;
            ParentId = parentId;
        }
        public ProductCategory()
        {
            product_Category_Rels = new List<Product_Category_Rel>();
        }
        public string Title { get; private set; }
        public string Slug { get; private set; }
        public int ParentId { get; private set; }
        public ICollection<Product_Category_Rel> product_Category_Rels { get; set; }
        public void Edit(string title, string slug, int parent)
        {
            Title = title;
            Slug = slug;
        }
    }


}
