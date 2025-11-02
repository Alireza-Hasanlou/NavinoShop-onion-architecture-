using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Application.Contract.BanerService.Query;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Site.Baners
{
    public class IndexModel : PageModel
    {
        private readonly IBanerQueryService _banerQueryService;

        public IndexModel(IBanerQueryService banerQueryService)
        {
            _banerQueryService = banerQueryService;
        }
        public List<BanerForAdminQueryModel> Baners { get; set; }
        public async Task OnGet()
        {
            Baners = await _banerQueryService.GetAllForAdmin();
        }
    }
}
