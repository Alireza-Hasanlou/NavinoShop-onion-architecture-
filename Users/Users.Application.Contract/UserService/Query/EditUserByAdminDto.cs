using Microsoft.AspNetCore.Http;
using Shared.Application.Validations;
using Shared.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using Users.Application.Contract.UserService.Command;

namespace Users.Application.Contract.UserService.Query
{
    public class EditUserByAdminDto
    {
        public int Id { get; set; }
        [Display(Name = "نام کامل")]
        [MaxLength(255, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        public string? FullName { get; set; }
        [Display(Name = "شماره همراه")]
        [MobileValidation(ErrorMessage = ValidationMessages.MobileErrorMessage)]
        public string Mobile { get; set; }
        [Display(Name = "ایمیل")]
        [MaxLength(255, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        public string? Email { get; set; }
        [Display(Name = "کلمه عبور")]
        [MaxLength(8, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        [MinLength(5, ErrorMessage = ValidationMessages.MinLengthMessage)]
        public string? Password { get; set; }
        public string AvatarName { get; set; }
        [Display(Name = "تصویرکاربری")]
        public IFormFile? AvatarFile { get; set; }
        [Display(Name = "جنسیت")]
        public Gender UserGender { get; set; }
    }
}