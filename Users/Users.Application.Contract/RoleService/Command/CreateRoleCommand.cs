using System.ComponentModel.DataAnnotations;
using Utility.Shared.Application;

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
