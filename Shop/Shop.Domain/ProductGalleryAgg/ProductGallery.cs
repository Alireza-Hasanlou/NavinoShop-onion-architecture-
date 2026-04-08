using Shared.Domain;
using Shop.Domain.ProductAgg;

namespace Shop.Domain.ProductGalleryAgg
{
    public class ProductGallery : BaseEntity<int>
    {
        public int ProductId { get; private set; }
        public string ImageName { get; private set; }
        public string ImageAlt { get; set; }
        public Product Product { get; set; }

        public ProductGallery(int productId, string imageName, string imageAlt)
        {
            ProductId = productId;
            ImageName = imageName;
            ImageAlt = imageAlt;

        }
        public ProductGallery()
        {
            Product = new();
        }
    }

}
