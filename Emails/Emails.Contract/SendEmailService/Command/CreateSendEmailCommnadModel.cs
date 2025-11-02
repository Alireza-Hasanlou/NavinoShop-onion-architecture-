using Shared.Application;
using Shared.Application.Validations;
using System.ComponentModel.DataAnnotations;

namespace Emails.Application.Contract.SendEmailService.Command
{
    public class CreateSendEmailCommnadModel
    {
        [Display(Name = "متن ایمیل")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        public string Text { get; set; }
        [Display(Name = "عنوان ایمیل")]
        [MaxLength(250,ErrorMessage = ValidationMessages.MaxLengthMessage)]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        public string Title { get; set; }
    }
}
