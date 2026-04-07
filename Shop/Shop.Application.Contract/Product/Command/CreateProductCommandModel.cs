using Shared.Application.BaseCommands;
using Shared.Application.Validations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shop.Application.Contract.Product.Command
{
    public class CreateProductCommandModel : Text_ShortDescription_Title_Slug_Image_ImageAlt
    {
        [DisplayName("وزن")]
        [Required(ErrorMessage =ValidationMessages.RequiredMessage)]
        public int Weight { get; set; }
        public List<int> CategoryIds { get; set; }
    }
}