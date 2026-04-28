using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Contract.ProductFeature.Query;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.ProductFeatures
{
    public class IndexModel : PageModel
    {
        private readonly IProductFeatureQueries _productFeatureQueries;

        public IndexModel(IProductFeatureQueries productFeatureQueries)
        {
            _productFeatureQueries = productFeatureQueries;
        }
        public ProductFeatureAdminPage PrductFeatures { get; set; }
        public async Task<IActionResult> OnGet(int ProductId)
        {
            if (ProductId < 1)
                return Redirect("/Admin/Products/Index");
            PrductFeatures = await _productFeatureQueries.GetProdutFeaturesForAdmin(ProductId);
            return Page();
        }
    }
}
