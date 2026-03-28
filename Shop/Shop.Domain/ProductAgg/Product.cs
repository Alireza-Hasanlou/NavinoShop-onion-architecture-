
using Shared.Domain;
using Shop.Domain.ProductCategoryAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ProductAgg
{
    public partial class Product : BaseEntityCreateUpdateActive<int>
    {
        public Product(string title, string imageName, string imageAlt, string slug, int categoryId)
        {
            Title = title;
            ImageName = imageName;
            ImageAlt = imageAlt;
            Slug = slug;
            CategoryId = categoryId;
            ProductCategory = new();
           
        }
        public Product()
        {

        }
        public string Title { get; private set; }
        public string ImageName { get; private set; }
        public string ImageAlt { get; private set; }
        public string ShortDescription { get; private set; }
        public string Description { get; private set; }
        public string Slug { get; private set; }
        public int CategoryId { get; private set; }
        public ICollection<ProductCategory> ProductCategories  { get; private set; }
        public ICollection<ProductFreature> ProductFreatures { get; private set; }
        public ICollection<ProductGallery> ProductGalleries { get; private set; }

        public void Edit(string title, string imageName, string imageAlt, string slug, int parent)
        {
            Title = title;
            ImageName = imageName;
            ImageAlt = imageAlt;
            Slug = slug;
        }
    }
}
