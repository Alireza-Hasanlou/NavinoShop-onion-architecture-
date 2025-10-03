using Shared.Application.BaseCommands;
using Shared.Application.Validations;
using System.ComponentModel.DataAnnotations;

namespace Site.Application.Contract.SitePageService.Command
{
	public class CreateSitePageCommnadModel : Title_Slug
	{
		[Display(Name = "توضیح")]
		[Required(ErrorMessage = ValidationMessages.RequiredMessage)]
		public string Text { get; set; }
	}
}
