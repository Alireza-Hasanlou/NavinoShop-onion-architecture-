using System.ComponentModel.DataAnnotations;
using Utility.Shared.Application;
namespace NavinoShop.WebApplication.Utility.ViewModels
{
    public class RoleTitileViewModel
    {
        [Display(Name = "  عنوان نقش")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [MaxLength(255, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        public string RoleTitle { get; set; }
    }
}
