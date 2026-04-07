using Shared.Application.Validations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shop.Application.Contract.ProductFeature.Command
{
    public class CreateProductFeatureCommandModel
    {
        public int ProductId { get; set; }
        [DisplayName("عنوان")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        public string Title { get; set; }
        [DisplayName("عنوان")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        public string Value { get; set; }
    }
}
