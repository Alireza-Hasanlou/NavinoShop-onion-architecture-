using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PostModule.Application.Contract.PostApplication;
using PostModule.Application.Contract.PostQuery;
using PostModule.Application.Contract.StateQuery;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Post.City
{
    public class IndexModel : PageModel
    {
        public ICityQuery _cityQuery { get; set; }

        public IndexModel(ICityQuery cityQuery)
        {
            _cityQuery = cityQuery;
        }
        public StateDetailQueryModel StateDetail { get; set; }
        public async Task<IActionResult> OnGet(int id)
        {           
            StateDetail = await _cityQuery.GetStateDetail(id);
            ViewData["title"] = $"شهر های استان {StateDetail.Name}";
            return Page();
        }
    }
}
