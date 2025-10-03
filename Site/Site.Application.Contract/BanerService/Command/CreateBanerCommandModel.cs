using Shared.Application.BaseCommands;
using Shared.Application.Validations;
using Shared.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Site.Application.Contract.BanerService.Command
{
    public class CreateBanerCommandModel:Image_ImageAlt
    {
  
        [Display(Name = "لینک مقصد")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [MaxLength(900, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        public string Url { get; set; }
        [Display(Name = "جایگاه")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        public BanerState State { get; set; }
    }
}
