using Shared.Application.BaseCommands;

namespace Shop.Application.Contract.ProductCategory.Commands
{
    public class EditProductCategoryCommandModel : Title_Slug_Image_ImageAlt
    {
        public string ImageName { get; set; }
        public int ProductCategoryId { get; set; }
        public int ParentId { get; set; }
    }
}
