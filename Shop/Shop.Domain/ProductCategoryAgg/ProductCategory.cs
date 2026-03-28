using Shared.Domain;
using Shop.Domain.ProductAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ProductCategoryAgg
{
    public class ProductCategory : BaseEntityCreateUpdateActive<int>
    {
        public ProductCategory(string title, string imageName, string imageAlt, string slug, int parent)
        {
            Title = title;
            ImageName = imageName;
            ImageAlt = imageAlt;
            Slug = slug;
            Parent = parent;
        }
        public ProductCategory()
        {

        }
        public string Title { get; private set; }
        public string ImageName { get; private set; }
        public string ImageAlt { get; private set; }
        public string Slug { get; private set; }
        public int Parent { get; private set; }
        public ICollection<Product> Products { get; set; }
        public void Edit(string title, string imageName, string imageAlt, string slug, int parent)
        {
            Title = title;
            ImageName = imageName;
            ImageAlt = imageAlt;
            Slug = slug;
        }
    }


}
