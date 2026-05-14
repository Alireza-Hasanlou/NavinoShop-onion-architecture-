using Microsoft.AspNetCore.Mvc;
using Shop.Application.Contract.ProductCategory.Query;

namespace NavinoShop.WebApplication.ViewComponents
{
    public class CategoriesViewComponent:ViewComponent
    {
        private readonly IProductCategoryQueries _productCategoryQueries;

        public CategoriesViewComponent(IProductCategoryQueries productCategoryQueries)
        {
            _productCategoryQueries = productCategoryQueries;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _productCategoryQueries.GetCategoriesForAddProduct();
            return View(categories);
        }
    }
}
