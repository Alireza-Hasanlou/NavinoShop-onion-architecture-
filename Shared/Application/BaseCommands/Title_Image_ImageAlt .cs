using Microsoft.AspNetCore.Http;
using Shared.Application.Validations;
using System.ComponentModel.DataAnnotations;

namespace Shared.Application.BaseCommands
{
    public class Title_Image_ImageAlt
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [MaxLength(250, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        public string Title { get; set; }
        [Display(Name = "تصویر")]
		public IFormFile? ImageFile { get; set; }
		[Display(Name = "Alt تصویر")]
		[Required(ErrorMessage = ValidationMessages.RequiredMessage)]
		[MaxLength(150, ErrorMessage = ValidationMessages.MaxLengthMessage)]
		public string ImageAlt { get; set; }
	}
}
