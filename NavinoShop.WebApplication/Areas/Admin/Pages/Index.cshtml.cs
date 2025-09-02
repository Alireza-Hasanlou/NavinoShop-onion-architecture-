
using Microsoft.AspNetCore.Mvc.RazorPages;
using NavinoShop.WebApplication.Utility;

namespace NavinoShop.WebApplication.Areas.Admin.Pages
{
    [PermissionChecker(1)]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
