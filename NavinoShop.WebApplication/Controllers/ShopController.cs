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
        public IActionResult Products(string search)
        {
          
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Products(int minPrice = 0, int maxprice = 0, ProductSort sort = ProductSort.جدیدترین,
                                               string categorySlug = "", int pageId = 1, string search = "")
        {
            try
            {
                ViewBag.CurrentSort = sort;
                // اجرای کوئری با پارامترهای دریافت شده
                var result = await _productUiQueryService.GetProducts(minPrice, maxprice, sort, categorySlug, pageId, search);


                // بررسی وجود نتیجه
                if (result == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "محصولی یافت نشد",
                        products = new List<object>(),
                        pagination = new { totalPages = 0, currentPage = pageId }
                    });
                }

                // برگرداندن نتیجه با ساختار مناسب برای فرانت‌اند
                return Json(new
                {
                    success = true,
                    products = result.Products,
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
                // لاگ خطا
                // _logger.LogError(ex, "خطا در دریافت محصولات");

                return Json(new
                {
                    success = false,
                    message = "خطا در دریافت محصولات",
                    error = ex.Message // فقط در محیط توسعه
                });
            }
        }

    }
}
