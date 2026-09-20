using Microsoft.AspNetCore.Mvc;
using NavinoShop.WebApplication.Utility.ViewModels;
using Query.Contract.UI.Products;
using Site.Application.Contract.MenuService.Query;

namespace NavinoShop.WebApplication.ViewComponents
{
    public class BestSellersByCategoryViewComponent : ViewComponent
    {
        private readonly IMenuQueryService _menuQueryService;
        private readonly IProductUiQueryService _productUiQueryService;

        public BestSellersByCategoryViewComponent(IMenuQueryService menuQueryService,
            IProductUiQueryService productUiQueryService)
        {
            _menuQueryService = menuQueryService;
            _productUiQueryService = productUiQueryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var Menus = await _menuQueryService.GetProductCategoryMenueForIndexAsync();
            if (Menus.Any())
            {
                var products = await _productUiQueryService.GetBestSellersByCategoryAsync(Menus.First().Title);
                return View(new BestSellersByCategoryViewModel
                {
                    Menus = Menus,
                    Products = products
                });
            }

            return View(new BestSellersByCategoryViewModel
            {
                Menus = new()
            });

        }
    }
}
