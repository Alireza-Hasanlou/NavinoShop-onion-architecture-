using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Query.Contract.Admin.Seller;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Sellers
{
    public class SalesRequestsModel : PageModel
    {
        private readonly IAdminSellerQueryService _adminSellerQueryService;

        public SalesRequestsModel(IAdminSellerQueryService adminSellerQueryService)
        {
            _adminSellerQueryService = adminSellerQueryService;
        }
        public List<SellersRequrstAdminQueryModel> Sellers { get; set; }
        public async Task<IActionResult> OnGet()
        {
            Sellers = await _adminSellerQueryService.GetAllSalesRequrstForAdmin();
            return Page();

        }
    }
}
