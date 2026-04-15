using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Query.Contract.Admin.Seller;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Sellers
{

    public class RequestDetailModel : PageModel
    {
        private readonly IAdminSellerQueryService _adminSellerQueryService;

        public RequestDetailModel(IAdminSellerQueryService adminSellerQueryService)
        {
            _adminSellerQueryService = adminSellerQueryService;
        }
        public SellerRequestDetailQueryModel seller { get; set; }
        public async Task<IActionResult> OnGet(int Id)
        {
            if (Id < 0)
                return RedirectToPage("Index");
            seller = await _adminSellerQueryService.GetSellerRequestDetail(Id);
            if (seller == null)
                return RedirectToPage("Index");
            return Page();
        }
    }
}
