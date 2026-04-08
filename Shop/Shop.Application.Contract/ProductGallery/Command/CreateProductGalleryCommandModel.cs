using Shared.Application.BaseCommands;

namespace Shop.Application.Contract.ProductGallery.Command
{
    public class CreateProductGalleryCommandModel : Image_ImageAlt
    {
        public int ProductId { get; set; }
    }


}
