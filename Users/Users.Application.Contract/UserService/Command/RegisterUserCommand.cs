
using System.ComponentModel.DataAnnotations;
using Utility.Shared.Application;
using Utility.Shared.Application.Validations;

namespace Users.Application.Contract.UserService.Command
{
    public class RegisterUserCommand
    {
        [Display(Name = "شماره همراه")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [MobileValidation(ErrorMessage = ValidationMessages.MobileErrorMessage)]
        public string Mobile { get; set; }

    }
}
