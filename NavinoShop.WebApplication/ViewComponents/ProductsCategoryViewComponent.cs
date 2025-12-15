using Microsoft.AspNetCore.Mvc;
using Site.Application.Contract.MenuService.Query;

namespace NavinoShop.WebApplication.ViewComponents
{
    public class ProductsCategoryViewComponent:ViewComponent
    {
        private readonly IMenuQueryService _menuQueryService;

        public ProductsCategoryViewComponent(IMenuQueryService menuQueryService)
        {
            _menuQueryService = menuQueryService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories= await _menuQueryService.GetProductCategoriesForIndexAsync();

            return View(categories);
        }
    }
}
