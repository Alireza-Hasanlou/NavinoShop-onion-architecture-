using Shared.Application;
using System.ComponentModel.DataAnnotations;
using Utility.Shared.Application;

namespace Emails.Application.Contract.SensEmailService.Command
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
