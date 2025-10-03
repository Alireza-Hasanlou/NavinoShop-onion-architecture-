using Microsoft.AspNetCore.Http;
using Shared.Application;
using Shared.Application.BaseCommands;
using Shared.Application.Validations;
using System.ComponentModel.DataAnnotations;

namespace Site.Application.Contract.SliderService.Command
{
    public class CreateSliderCommandModel:Image_ImageAlt
    {
        [Display(Name = "لینک مقصد")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [MaxLength(900, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        public string Url { get; set; }
    } 
}
