using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PostModule.Application.Contract.CityService;
using PostModule.Application.Contract.StateQuery;
using PostModule.Application.Contract.UserPostApplication.Query;
using PostModule.Query.Services;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Post.City
{
    public class CreateModel : PageModel
    {
        private readonly ICityCommandService _cityCommandService;
        private readonly IStateQueryService  _stateQueryService;

        public CreateModel(ICityCommandService cityCommandService, IStateQueryService stateQueryService)
        {
            _cityCommandService = cityCommandService;
            _stateQueryService = stateQueryService;
        }

        [BindProperty]
        public CreateCityModel Createcity { get; set; }
        public async Task<IActionResult> OnGet(int stateid)
        {
            var stateTitle = await _stateQueryService.GetStateTitle(stateid);
            ViewData["title"] = $"افزودن شهر جدید به استان {stateTitle}";
            Createcity = new CreateCityModel { StateId = stateid };
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _cityCommandService.CreateAsync(Createcity);
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
