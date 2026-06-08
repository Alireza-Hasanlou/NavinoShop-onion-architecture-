using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Query.Contract.Admin.Products;


namespace NavinoShop.WebApplication.Areas.Admin.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IAdminProductsQueryService _adminProductsQueryService;

        public IndexModel(IAdminProductsQueryService adminProductsQueryService)
        {
            _adminProductsQueryService = adminProductsQueryService;
        }

        public ProductsForAdminPaging Products { get; set; }
        public async Task<IActionResult> OnGet(int pageId = 1, int take = 10, string filter = "",int categoryId=0)
        {

            Products = await _adminProductsQueryService.GetAllProductsForAdmin(pageId, take, filter,categoryId);
            return Page();
        }

    }
}
