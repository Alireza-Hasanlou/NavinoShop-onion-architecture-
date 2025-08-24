using System.ComponentModel.DataAnnotations;
using Utility.Shared.Application;
using Utility.Shared.Application.BaseCommands;

namespace Users.Application.Contract.PermissionService.Command
{
    public class CreatePermissionCommand
    {

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [MaxLength(250, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        [MaxLength(500, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        public string? Description { get; set; }

    }
}