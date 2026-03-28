using Shared.Domain;
namespace Shop.Domain.ProductAgg
{
        public class ProductGallery : BaseEntity<int>
        {
            public int ProductCategory { get; private set; }
            public string ImageName { get; private set; }
            public string ImageAlt { get; set; }
            public Product Product { get; set; }

            public ProductGallery(int productCategory, string imageName, string imageAlt)
            {
                ProductCategory = productCategory;
                ImageName = imageName;
                ImageAlt = imageAlt;
                Product = new();
            }
            public ProductGallery()
            {
                
            }
        }
    
}
