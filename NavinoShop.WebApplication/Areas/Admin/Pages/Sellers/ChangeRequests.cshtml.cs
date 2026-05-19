using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Contract.Seller.Command;
using Shop.Application.Contract.Seller.Query;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Sellers
{
    public class ChangeRequestsModel : PageModel
    {
        private readonly ISellerQueries _sellerQueries;

        public ChangeRequestsModel(ISellerQueries sellerQueries)
        {
            _sellerQueries = sellerQueries;
        }
        public List< SellersChangeRequestQueryModel> Requests { get; set; }
        public async Task<IActionResult> OnGet()
        {
            Requests = await _sellerQueries.GetChangeRequests();
            return Page();

        }
    }
}