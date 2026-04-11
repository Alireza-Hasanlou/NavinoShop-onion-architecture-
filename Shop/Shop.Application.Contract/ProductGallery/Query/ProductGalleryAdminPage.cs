using Shared;

namespace Shop.Application.Contract.ProductGallery.Query
{
    public class ProductGalleryAdminPage
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public List<ProductGalleryForAdminQueryModel> ProductGalleries { get; set; }

    }
}
