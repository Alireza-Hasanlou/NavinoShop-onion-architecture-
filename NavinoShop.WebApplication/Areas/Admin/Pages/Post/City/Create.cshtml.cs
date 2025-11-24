using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PostModule.Application.Contract.CityApplication;
using PostModule.Application.Contract.StateQuery;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Post.City
{
    public class CreateModel : PageModel
    {
        private readonly ICityApplication _cityApplication;
        private readonly ICityQuery _cityQuery;

        public CreateModel(ICityApplication cityApplication, ICityQuery cityQuery)
        {
            _cityApplication = cityApplication;
            _cityQuery = cityQuery;
        }

        [BindProperty]
        public CreateCityModel Createcity { get; set; }
        public async Task<IActionResult> OnGet(int stateid)
        {
            var stateTitle = await _cityQuery.GetStateTitle(stateid);
            ViewData["title"] = $"افزودن شهر جدید به استان {stateTitle}";
            Createcity = new CreateCityModel { StateId = stateid };
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _cityApplication.CreateAsync(Createcity);
            if (result.Success)
            {
                TempData["success"] = "شهر جدید با موفقیت ایجاد شد";
                return Redirect($"/Admin/Post/city/Index?id={Createcity.StateId}");
            }
            ModelState.AddModelError($"Createcity.{result.ModelName}", result.Message);
            return Page();
        }
    }
}
