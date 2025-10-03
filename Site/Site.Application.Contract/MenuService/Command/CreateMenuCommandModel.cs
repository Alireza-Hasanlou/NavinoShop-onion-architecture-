
using Shared.Application.Validations;
using Shared.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Site.Application.Contract.MenuService.Command
{
    public class CreateMenuCommandModel : UbsertMenuCommandModel
    {
        [Display(Name = "نوع منو")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        public MenuStatus Status { get; set; }
       
    }
}
