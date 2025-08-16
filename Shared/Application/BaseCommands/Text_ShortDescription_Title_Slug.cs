using Utility.Shared.Application.BaseCommands;
using System.ComponentModel.DataAnnotations;

namespace Utility.Shared.Application.BaseCommands
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
