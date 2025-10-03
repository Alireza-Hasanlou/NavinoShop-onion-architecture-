using System.ComponentModel.DataAnnotations;
using Shared.Application.Validations;

namespace Shared.Application.BaseCommands
{
    public class Text_ShortDescription_Title_Slug : Title_Slug
	{
		[Display(Name = "توضیح مختصر")]
		[Required(ErrorMessage = ValidationMessages.RequiredMessage)]
		[MaxLength(600, ErrorMessage = ValidationMessages.MaxLengthMessage)]
		public string ShortDescription { get; set; }
		[Display(Name = "توضیح")]
		[Required(ErrorMessage = ValidationMessages.RequiredMessage)]
		public string Text { get; set; }
	}
}
