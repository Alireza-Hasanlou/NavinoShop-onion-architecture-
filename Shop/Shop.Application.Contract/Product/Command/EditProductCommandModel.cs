using Shared.Application.BaseCommands;
using Shop.Application.Contract.ProductCategory.Query;
using System.ComponentModel;

namespace Shop.Application.Contract.Product.Command
{
    public class EditProductCommandModel : Text_ShortDescription_Title_Slug_Image_ImageAlt
    {
        public int Id { get; set; }
        [DisplayName("وزن")]
        public int Weight { get; set; }
        public string ImageName { get; set; }
        public List<int>? SelectedCategory { get; set; }
        public List<ProductCategoryForAddProduct>? Categories { get; set; }
    }


}