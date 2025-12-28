using Microsoft.AspNetCore.Http;
using Shared.Application.Validations;
using System.ComponentModel.DataAnnotations;

namespace Site.Application.Contract.MenuService.Command
{
    public class UbsertMenuCommandModel
    {
        [Display(Name = " تصویر برای منوی گروه های محصولات در اندازه 256*451 - تصویر برای منو وبلاگ در اندازه 284*180")]
        public IFormFile? ImageFile { get; set; }
        [Display(Name = "Alt تصویر")]
        [MaxLength(250, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        public string? ImageAlt { get; set; }
        [Display(Name = "شماره برای ترتیب بندی")]
        public int Number { get; set; } = 0;
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [MaxLength(250, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        public string Title { get; set; }
        [Display(Name = "لینک مقصد (برای منو هایی که زیر منو دارند # وارد کنید )")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [MaxLength(250, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        public string Url { get; set; }
    }
}
