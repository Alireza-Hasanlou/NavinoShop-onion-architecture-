using Comments.Application.Contract.CommentService.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NavinoShop.WebApplication.Utility.ViewModels;
using Newtonsoft.Json;
using Query.Contract.UI.Comments;
using Query.Contract.UI.Products;
using Shared.Application.Auth;
using Shared.Domain.Enums;
using Shared.Ui.Enums;
using Shop.Application.Contract.ProductView;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Controllers
{


    public class ShopController : Controller
    {
        private readonly IProductUiQueryService _productUiQueryService;
        private readonly IProductViewCommands _productViewCommands;
        private readonly IAuthService _authService;

        public ShopController(IProductUiQueryService productUiQueryService, IProductViewCommands productViewCommands , IAuthService authService)
        {
            _productUiQueryService = productUiQueryService;
            _productViewCommands = productViewCommands;
            _authService = authService;
        }

        [HttpGet]
        [Route("/Products")]
        [Route("/Products/{categorySlug}")]
        [Route("/Products/Seller/{seller}")]
        [Route("/Products/{Seller}/{categorySlug}")]
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


            var sessionId = HttpContext.Session.Id;
            var UserId = _authService.GetLoginUserId();
            var alreadyViewed = await _productViewCommands.IsExistProductViewAsync(prodcut.ProductId, sessionId);    
            if(!alreadyViewed)
            {
                await _productViewCommands.CreateAsync(new CreateProductViewCommandModel(UserId, prodcut.ProductId, sessionId));
            }
            return View(prodcut);
        }

        [HttpGet]
        public async Task<IActionResult> GetProdcutOtherSellers(int SellerId, string productSlug)
        {
            if (string.IsNullOrEmpty(productSlug))
                return Json(new { success = false, message = "productSlug is required", data = new List<object>() });

            var products = await _productUiQueryService.GetProductOtherSellers(SellerId, productSlug);

            if (products == null || !products.Any())
            {
                return Json(new { success = false, message = "No other sellers found", data = new List<object>() });
            }

            return Json(new { success = true, data = products });
        }


    }
}
