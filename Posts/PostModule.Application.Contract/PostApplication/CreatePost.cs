using Shared.Application;
using Shared.Application.Validations;
using System.ComponentModel.DataAnnotations;

namespace PostModule.Application.Contract.PostApplication
{
    public class CreatePost
    {
        [Display(Name = "عنوان پست")]
        [Required(  ErrorMessage = ValidationMessages.RequiredMessage)]
        public string Title { get; set; }
        [Display(Name = "توضیحات تحویل")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        public string Status { get; set; }
        [Display(Name = "اضافه بار هر کیلوگرم درون شهری تهران (تومان)")]
        [MinLength(1000,ErrorMessage = ValidationMessages.RequiredMessage)]
        public int TehranPricePlus { get; set; }
        [Display(Name = "اضافه بار هر کیلوگرم درون شهری مراکز استان ها (تومان)")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        public int StateCenterPricePlus { get; set; }
        [Display(Name = "اضافه بار هر کیلوگرم درون شهری شهرستان ها (تومان)")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        public int CityPricePlus { get; set; }
        [Display(Name = "اضافه بار هر کیلوگرم درون استانی (تومان)")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        public int InsideStatePricePlus { get; set; }
        [Display(Name = "اضافه بار هر کیلوگرم برون استانی هم جوار (تومان)")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        public int StateClosePricePlus { get; set; }
        [Display(Name = "اضافه بار هر کیلوگرم برون استانی غیر هم جوار (تومان)")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        public int StateNonClosePricePlus { get; set; }
        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        public string Description { get; set; }
    }
}
