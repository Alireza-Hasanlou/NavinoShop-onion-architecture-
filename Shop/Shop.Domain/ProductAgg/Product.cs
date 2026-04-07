
using Shared.Domain;
using Shop.Domain.Product_SellerAgg;
using Shop.Domain.ProductCategoryAgg;
using Shop.Domain.ProductFreatureAgg;
using Shop.Domain.ProductGalleryAgg;
using Shop.Domain.Relations.ProductCategoryRel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ProductAgg
{
    public partial class Product : BaseEntityCreateUpdateActive<int>
    {

        public Product()
        {
            Poduct_Category_Rels = new List<Product_Category_Rel>();
            ProductFreatures = new List<ProductFreature>();
            ProductGalleries = new List<ProductGallery>();
            ProductSells = new List<Product_Seller>();
        }

        public Product(string title, string imageName, string imageAlt, string shortDescription, string description, int weight, string slug)
        {
            Title = title;
            ImageName = imageName;
            ImageAlt = imageAlt;
            ShortDescription = shortDescription;
            Description = description;
            Weight = weight;
            Slug = slug;
        }

        public string Title { get; private set; }
        public string ImageName { get; private set; }
        public string ImageAlt { get; private set; }
        public string ShortDescription { get; private set; }
        public string Description { get; private set; }
        public int Weight { get; private set; }
        public string Slug { get; private set; }

        public ICollection<Product_Category_Rel> Poduct_Category_Rels { get; private set; }
        public ICollection<ProductFreature> ProductFreatures { get; private set; }
        public ICollection<ProductGallery> ProductGalleries { get; private set; }
        public ICollection<Product_Seller> ProductSells { get; private set; }


        public void EditProductCategoryRelation(List<Product_Category_Rel> product_Category_Rels)
        {
            Poduct_Category_Rels.Clear();
            Poduct_Category_Rels = product_Category_Rels;
        }
        public void Edit(string title, string imageName, string imageAlt, string shortDescription, string description, int weight, string slug)
        {
            Title = title;
            ImageName = imageName;
            ImageAlt = imageAlt;
            ShortDescription = shortDescription;
            Description = description;
            Weight = weight;
            Slug = slug;
        }
    }
}
