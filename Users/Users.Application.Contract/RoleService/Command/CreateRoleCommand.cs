using Shared.Application.Validations;
using System.ComponentModel.DataAnnotations;

namespace Users.Application.Contract.RoleService.Command
{
    public class CreateRoleCommand
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [MaxLength(255, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        public string Title { get; set; }
    }
}
