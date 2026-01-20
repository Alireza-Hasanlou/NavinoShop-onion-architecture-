using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Emails.Application.Contract.MessageUserService.Command
{
    public class CreateMessageUserCommandModel
    {
        public int UserId { get; set; }
        [DisplayName("نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا نام خود را وارد کنید")]
        public string FullName { get; set; }
        [DisplayName("موضوع")]
        [Required(ErrorMessage ="لطفا موضوع پیام را وارد کنید")]
        public string Subject { get; set; }
        [DisplayName("شماره تماس")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(11,ErrorMessage ="شماره تماس نمیتواند بیشتر از 11 رقم باشد")]
        [MinLength(11, ErrorMessage = "شماره تماس نمیتواند کمتر از 11 رقم باشد")]
        public string? PhoneNumber { get; set; }
        [DisplayName("ایمیل")]
        [DataType(DataType.EmailAddress,ErrorMessage ="ایمیل وارد شده صحیح نیست ")]
        [Required(ErrorMessage = "لطفا ایمیل خود را وارد کنید")]
        public string? Email { get; set; }
        [DisplayName("ایمیل")]
        [Required(ErrorMessage = "لطفا پیام خود را وارد کنید")]
        [MaxLength(600, ErrorMessage = " پیام نمیتواند بیشتر از 600 کاراکتر باشد")]
        public string Message { get; set; }
    
    }
}
