using Shared.Application.Validations;
using System.ComponentModel.DataAnnotations;

namespace Users.Application.Contract.RoleService.Query
{
    public class PermissionQueryModel
    {
        public int Id { get; set; }
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [MaxLength(255, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        public string Title { get;  set; }
     
    }
}