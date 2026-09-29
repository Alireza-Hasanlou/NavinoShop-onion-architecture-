using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Query.Contract.Admin.Data;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Chart
{
    public class WeeklySalesModel : PageModel
    {
        private readonly IAdminDataQueryService _siteDataQuery;

        public WeeklySalesModel(IAdminDataQueryService siteDataQuery)
        {
            _siteDataQuery = siteDataQuery;
        }

        public async Task<IActionResult> OnGetMonthlySalesAsync()
        {
            var result = await _siteDataQuery.GetMonthlySalesAsync();

            return new JsonResult(result);
        }

        public async Task<IActionResult> OnGetWeeklySalesAsync()
        {
            var result = await _siteDataQuery.GetWeeklySalesAsync();

            return new JsonResult(result);
        }
    }
}
