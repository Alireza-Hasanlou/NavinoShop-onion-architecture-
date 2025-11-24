using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PostModule.Application.Contract.PostApplication;
using PostModule.Application.Contract.PostQuery;
using PostModule.Application.Contract.StateQuery;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Post.State
{
    public class IndexModel : PageModel
    {
        public ICityQuery _cityQuery { get; set; }

        public IndexModel(ICityQuery cityQuery)
        {
            _cityQuery = cityQuery;
        }
        public List<StateAdminQueryModel> States { get; set; }
        public async Task OnGet()
        {
            States = await _cityQuery.GetStatesForAdmin();
        }
    }
}
