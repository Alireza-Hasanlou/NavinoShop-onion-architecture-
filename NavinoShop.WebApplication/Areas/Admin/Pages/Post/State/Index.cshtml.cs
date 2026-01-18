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
        public IStateQueryService _stateQueryService{ get; set; }

        public IndexModel(IStateQueryService stateQueryService)
        {
            _stateQueryService = stateQueryService;
        }

        public List<StateAdminQueryModel> States { get; set; }
        public async Task OnGet()
        {
            States = await _stateQueryService.GetStatesForAdmin();
        }
    }
}
