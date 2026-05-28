using Microsoft.AspNetCore.Http;
using Shared.Application.Validations;
using System.ComponentModel.DataAnnotations;

namespace Shop.Application.Contract.Seller.Command
{
    public class EditSellerQueryModel
    {
        public int SellerId { get; set; }
        public string? AvatarImageName { get; set; }
        public string? CoverImageName { get; set; }
        [Display(Name = "نام فروشگاه")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [MaxLength(355, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        public string Title { get; set; }
        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [MaxLength(600, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        public string Description { get; set; }
        [Display(Name = "جزییات آدرس")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        public string Address { get; set; }
        [Display(Name = "لینک نقشه گوگل")]
        public string? GoogleMapUrl { get; set; }
        [Display(Name = "کاور فروشگاه")]
        public IFormFile? CoverImage { get; set; }
        [Display(Name = "تصویر")]
        public IFormFile? ImageFile { get; set; }
        [Display(Name = "لینک چت وانس اپ")]
        [MaxLength(355, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        public string? WhatsApp { get; set; }
        [Display(Name = "لینک پیج تلگرام")]
        [MaxLength(355, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        public string? Telegram { get; set; }
        [Display(Name = "لینک پیج اینستاگرام")]
        [MaxLength(355, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        public string? Instagram { get; set; }
        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [MaxLength(11, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        [MinLength(11, ErrorMessage = ValidationMessages.MinLengthMessage)]
        public string Phone1 { get; set; }
        [Display(Name = "شماره تماس 2")]
        [MaxLength(11, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        [MinLength(11, ErrorMessage = ValidationMessages.MinLengthMessage)]
        public string? Phone2 { get; set; }
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [MaxLength(255, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        public string Email { get; set; }
        [Display(Name = "نام فروشگاه به انگلیسی")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [MaxLength(255, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        public string Slug { get; set; }

    }
}
