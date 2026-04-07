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
        public ProductCategory(string title, string imageName, string imageAlt, string slug, int parentId)
        {
            Title = title;
            ImageName = imageName;
            ImageAlt = imageAlt;
            Slug = slug;
            ParentId = parentId;
        }
        public ProductCategory()
        {
            product_Category_Rels = new List<Product_Category_Rel>();
        }
        public string Title { get; private set; }
        public string ImageName { get; private set; }
        public string ImageAlt { get; private set; }
        public string Slug { get; private set; }
        public int ParentId { get; private set; }
        public ICollection<Product_Category_Rel> product_Category_Rels { get; set; }
        public void Edit(string title, string imageName, string imageAlt, string slug, int parent)
        {
            Title = title;
            ImageName = imageName;
            ImageAlt = imageAlt;
            Slug = slug;
        }
    }


}
