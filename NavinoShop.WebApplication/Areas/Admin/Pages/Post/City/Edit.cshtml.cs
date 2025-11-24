using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PostModule.Application.Contract.CityApplication;
using PostModule.Application.Contract.PostApplication;
using PostModule.Application.Contract.PostQuery;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Post.City
{
    public class EditModel : PageModel
    {
        private readonly ICityApplication _cityApplication;

        public EditModel(ICityApplication cityApplication)
        {
            _cityApplication = cityApplication;
        }
        [BindProperty]
        public EditCityModel Editcity { get; set; }
        public async Task<IActionResult> OnGet(int Id)
        {
            if (Id < 1)
                return Page();

            Editcity = await _cityApplication.GetCityForEditAsync(Id);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _cityApplication.CreateAsync(Editcity);
            if (result.Success)
            {
                TempData["success"] = "شهر با موفقیت ویرایش شد";
                return Redirect("/Admin/Post/City/Index");
            }
            ModelState.AddModelError($"Editcity .{result.ModelName}", result.Message);
            return Page();
        }
    }
}
