using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PostModule.Application.Contract.CityService;
using Shared.Domain.Enums;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Post.City
{
    public class ChangeCityStatusModel : PageModel
    {
        private readonly ICityApplication _cityApplication;

        public ChangeCityStatusModel(ICityApplication cityApplication)
        {
            _cityApplication = cityApplication;
        }
        public async Task<JsonResult> OnGet(int id, CityStatus status)
        {
            if (id < 1)
                return new JsonResult(new { success = false });

            var result = await  _cityApplication.ChangeStatusAsync(id, status);
            if (result)
                return new JsonResult(new { success = true });

            return new JsonResult(new { success = false });

           
        } 

    }
}
