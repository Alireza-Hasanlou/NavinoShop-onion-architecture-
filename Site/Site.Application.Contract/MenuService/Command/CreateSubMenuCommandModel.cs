using Shared.Application;
using Shared.Application.Validations;
using Shared.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Site.Application.Contract.MenuService.Command
{
    public class CreateSubMenuCommandModel : UbsertMenuCommandModel
    {
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
        public int ParentId { get; set; }
        public MenuStatus ParentStatus { get; set; }
        public string ParentTitle { get; set; }
    }
}
