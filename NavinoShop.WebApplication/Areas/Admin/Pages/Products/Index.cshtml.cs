using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Contract.Product.Query;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductQueries _productQueries;

        public IndexModel(IProductQueries productQueries)
        {
            _productQueries = productQueries;
        }
        public ProductsForAdminPaging Products { get; set; }
        public async Task<IActionResult> OnGet(int pageId = 1, int take = 10, string filter = "",int categoryId=0)
        {

            Products = await _productQueries.GetAllProductsForAdmin(pageId, take, filter,categoryId);
            return Page();
        }

    }
}
