using Shared.Application.Validations;
using System.ComponentModel.DataAnnotations;

namespace PostModule.Application.Contract.CityService
{
	public class CreateCityModel
    {
        public int StateId { get; set; }
		[Display(Name = "نام شهر")]
		[Required(ErrorMessage = ValidationMessages.RequiredMessage)]
		[MaxLength(250, ErrorMessage = ValidationMessages.MaxLengthMessage)]
		public string Title { get; set; }
    }
}
