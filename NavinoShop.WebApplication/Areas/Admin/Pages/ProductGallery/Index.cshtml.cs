using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Contract.ProductFeature.Query;
using Shop.Application.Contract.ProductGallery.Query;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.ProductGallery
{
    public class IndexModel : PageModel
    {
        private readonly IProductGalleryQueries _productGalleryQueries;

        public IndexModel(IProductGalleryQueries productGalleryQueries)
        {
            _productGalleryQueries = productGalleryQueries;
        }
        public ProductGalleryAdminPage Gallery { get; set; }
        public async Task<IActionResult> OnGet(int ProductId)
        {
            if (ProductId < 1)
                return Redirect("/Admin/ProductGallery/Index");
            Gallery = await _productGalleryQueries.GetProductGalleriesForAdmin(ProductId);
            return Page();
        }
    }
}
