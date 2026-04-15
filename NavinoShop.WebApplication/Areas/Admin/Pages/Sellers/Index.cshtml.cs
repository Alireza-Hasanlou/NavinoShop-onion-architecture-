using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Query.Contract.Admin.Seller;
using Users.Application.Contract.RoleService.Query;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Sellers
{
    public class IndexModel : PageModel
    {
        private readonly IAdminSellerQueryService _adminSellerQueryService;

        public IndexModel(IAdminSellerQueryService adminSellerQueryService)
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
