using Comments.Application.Contract.CommentService.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Query.Contract.UI.Comments;
using Query.Contract.UI.Products;
using Shared.Application.Auth;
using Shared.Domain.Enums;
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
        [Route("/Products")]
        [Route("/Products/{categorySlug}")]
        [Route("{Seller:seller}")]                                   
        [Route("{Seller:seller}/Products/{categorySlug}")]          
        public async Task<IActionResult> Products(int minPrice = 0, int maxprice = 0, ProductSort sort = ProductSort.جدیدترین,
                                                   string categorySlug = "", string Seller = "", int pageId = 1, string search = "", bool IsAjax = false)
        {
            try
            {
                ViewBag.CurrentSort = sort;
                var result = await _productUiQueryService.GetProducts(minPrice, maxprice, sort, categorySlug, Seller, pageId, search);
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
        [Route("{seller}/Product/{productSlug}")]
        public async Task<IActionResult> Product(string seller, string productSlug)
        {
            if (string.IsNullOrEmpty(seller) || string.IsNullOrEmpty(productSlug))
                return NotFound();

            var prodcut = await _productUiQueryService.GetProductAsync(seller, productSlug);
            if (prodcut == null)
                return NotFound();

            return View(prodcut);
        }

        [HttpGet]
        public async Task<IActionResult> GetProdcutOtherSellers(int SellerId, string productSlug)
        {
            if (string.IsNullOrEmpty(productSlug))
                return Json(new { success = false, message = "productSlug is required", data = new List<object>() });

            var products = await _productUiQueryService.GetProductOtherSellers(SellerId,productSlug);

            if (products == null || !products.Any())
            {
                return Json(new { success = false, message = "No other sellers found", data = new List<object>() });
            }

            return Json(new { success = true, data = products });
        }
    }
}
