using Shared.Application.BaseCommands;

namespace Shop.Application.Contract.ProductCategory.Commands
{
    public class CreateProductCategoryCommandModel: Title_Slug
    {
        public int  ParentId { get; set; }
    }
}
