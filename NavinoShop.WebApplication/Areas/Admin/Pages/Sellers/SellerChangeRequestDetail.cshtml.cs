using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Query.Contract.Admin.Seller;
using Shop.Application.Contract.Seller.Query;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Sellers
{
    public class SellerChangeRequestDetailModel : PageModel
    {
        private readonly ISellerQueries _sellerQueries;

        public SellerChangeRequestDetailModel(ISellerQueries sellerQueries)
        {
            _sellerQueries = sellerQueries;
        }
        public RequestDetailQueryModel Details { get; set; }
        public async Task<IActionResult> OnGet(int Id)
        {
            Details = await _sellerQueries.GetSellerChangeRequestDeatail(Id);
            return Partial("_ChangeSellerPartial", Details);
        }
    }
}
