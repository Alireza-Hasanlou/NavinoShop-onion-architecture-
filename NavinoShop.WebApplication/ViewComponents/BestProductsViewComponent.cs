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

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var bestProducts = await _productUiQueryService.GetBestProducts();
            return View(bestProducts);
        }
    }
}
