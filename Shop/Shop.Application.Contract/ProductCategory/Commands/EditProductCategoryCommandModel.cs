using Shared.Application.BaseCommands;

namespace Shop.Application.Contract.ProductCategory.Commands
{
    public class EditProductCategoryCommandModel : Title_Slug
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
    }
}
