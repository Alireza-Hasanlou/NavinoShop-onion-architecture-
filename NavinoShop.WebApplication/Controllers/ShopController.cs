using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Query.Contract.UI.Products;
using Shared.Ui.Enums;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Controllers
{


    public class ShopController : Controller
    {
        private readonly IProductUiQueryService _productUiQueryService;

        public ShopController(IProductUiQueryService productUiQueryService)
        {
            _productUiQueryService = productUiQueryService;
        }

        [HttpGet]
        [Route("/Products/{categorySlug?}")]
        public async Task<IActionResult> Products(int minPrice = 0, int maxprice = 0, ProductSort sort = ProductSort.جدیدترین,
                                                   string categorySlug = "", int sellerId = 0, int pageId = 1, string search = "", bool IsAjax = false)
        {
            try
            {
                ViewBag.CurrentSort = sort;
                var result = await _productUiQueryService.GetProducts(minPrice, maxprice, sort, categorySlug, sellerId, pageId, search);
                if (!IsAjax)
                    return View(result);

                if (result == null || result.Products == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "محصولی یافت نشد",
                        products = new List<object>(),
                        breadCrumbs = result?.BreadCrumbs,
                        pagination = new { totalPages = 0, currentPage = pageId }
                    });
                }
                return Json(new
                {
                    success = true,
                    products = result.Products,
                    breadCrumbs = result.BreadCrumbs,
                    pagination = new
                    {
                        totalPages = result.PageCount,
                        currentPage = result.PageId,
                        totalCount = result.DataCount,
                        pageSize = 12
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "خطا در دریافت محصولات",
                    error = ex.Message
                });
            }
        }
        [HttpGet]
        [Route("/Product/{slug}")]
        public async Task<IActionResult> ProductDetail(string slug)
        {
            return View();
        }

    }
}
