using Microsoft.AspNetCore.Mvc;
using Query.Contract.UI.Products;
using Shared.Ui.Enums;

namespace NavinoShop.WebApplication.ViewComponents
{
    public class BestProductsViewComponent : ViewComponent
    {
        private readonly IProductUiQueryService _productUiQueryService;

        public BestProductsViewComponent(IProductUiQueryService productUiQueryService)
        {
            _productUiQueryService = productUiQueryService;
        }

        public async Task<IViewComponentResult> InvokeAsync(
            IndexPagesProduct sort = IndexPagesProduct.محصولات_منتخب)
        {
            var sortValue = HttpContext.Request.Query["sort"].ToString();
            if (Enum.TryParse<IndexPagesProduct>(sortValue, out var parsedSort))
            {
                sort = parsedSort;
            }

            var bestProducts = await _productUiQueryService.GetBestProducts(sort);
            return View(bestProducts);
        }
    }
}
