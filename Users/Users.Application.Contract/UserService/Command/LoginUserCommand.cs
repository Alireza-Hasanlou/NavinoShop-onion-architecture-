
using System.ComponentModel.DataAnnotations;
using Utility.Shared.Application;
using Utility.Shared.Application.Validations;

namespace Users.Application.Contract.UserService.Command
{
    public class LoginUserCommand
    {
        [Display(Name = "شماره همراه")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [MobileValidation(ErrorMessage = ValidationMessages.MobileErrorMessage)]
        public string Mobile { get; set; }
        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
