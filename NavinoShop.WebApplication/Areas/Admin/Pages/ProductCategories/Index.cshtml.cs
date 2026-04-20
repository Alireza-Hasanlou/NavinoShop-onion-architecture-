using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Contract.ProductCategory.Query;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.ProductCategories
{
    public class IndexModel : PageModel
    {
        private readonly IProductCategoryQueries _productCategoryQueries;

        public IndexModel(IProductCategoryQueries productCategoryQueries)
        {
            _productCategoryQueries = productCategoryQueries;
        }

        public ProductCategoryAdminPageQueryModel Categoreis { get; set; }
        public async Task<IActionResult> OnGet(int Id)
        {
            Categoreis = await _productCategoryQueries.GetCategoriesForAdmin(Id);
            return Page();
        }
    }
}
