using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NavinoShop.WebApplication.Utility.ViewModels;
using Newtonsoft.Json;
using Shop.Application.Contract.ProductSell.Query;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductSellQueries _productSellQueries;

        public CartController(IProductSellQueries productSellQueries)
        {
            _productSellQueries = productSellQueries;
        }

        [HttpGet]
        [Route("/Cart")]
        public IActionResult Index()
        {
            var cartItems = new List<CartItemViewModel>();
            var cartCookie = Request.Cookies["NavinoshoppingCart"];

            if (!string.IsNullOrEmpty(cartCookie))
            {
                try
                {
                    var cartData = JsonConvert.DeserializeObject<Dictionary<string, CartItem>>(cartCookie);

                    if (cartData != null && cartData.Any())
                    {
                        foreach (var item in cartData)
                        {
                            var cartItem = new CartItemViewModel
                            {
                                Id = int.Parse(item.Key),
                                Title = item.Value.Title ?? "بدون عنوان",
                                ImageName = item.Value.ImageName,
                                Price = item.Value.Price,
                                PriceAfterOff = item.Value.PriceAfterOff,
                                Seller = item.Value.Seller ?? "نام فروشنده",
                                Quantity = item.Value.quantity,
                                Amount = item.Value.Amount,
                                TotalPrice = (item.Value.PriceAfterOff > 0 && item.Value.PriceAfterOff < item.Value.Price)
                                    ? item.Value.PriceAfterOff * item.Value.quantity
                                    : item.Value.Price * item.Value.quantity
                            };

                            cartItems.Add(cartItem);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return View(cartItems);
                }
            }

            var viewModel = new CartViewModel
            {
                Items = cartItems,
                TotalAmount = cartItems.Sum(x => x.TotalPrice),
                TotalItems = cartItems.Sum(x => x.Quantity)
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetCartData()
        {
            try
            {
                var cartItems = new List<object>();
                var cartCookie = Request.Cookies["NavinoshoppingCart"];
                decimal totalAmount = 0;
                decimal totalDiscount = 0;
                int totalItems = 0;

                if (!string.IsNullOrEmpty(cartCookie))
                {
                    var cartData = JsonConvert.DeserializeObject<Dictionary<string, CartItem>>(cartCookie);

                    if (cartData != null && cartData.Any())
                    {
                        foreach (var item in cartData)
                        {
                            var price = item.Value.Price;
                            var priceAfterOff = item.Value.PriceAfterOff;
                            var hasDiscount = priceAfterOff > 0 && priceAfterOff < price;
                            var finalPrice = hasDiscount ? priceAfterOff : price;
                            var itemTotal = finalPrice * item.Value.quantity;
                            var discountAmount = hasDiscount ? (price - priceAfterOff) * item.Value.quantity : 0;

                            // Check stock availability
                            var hasStock = await _productSellQueries.ProductSellHaveAmount(int.Parse(item.Key));

                            cartItems.Add(new
                            {
                                id = int.Parse(item.Key),
                                title = item.Value.Title ?? "بدون عنوان",
                                imageName = item.Value.ImageName ,
                                price = price,
                                priceAfterOff = priceAfterOff,
                                seller = item.Value.Seller ?? "نام فروشنده",
                                quantity = item.Value.quantity,
                                amount = item.Value.Amount,
                                totalPrice = itemTotal,
                                hasDiscount = hasDiscount,
                                hasStock = hasStock
                            });

                            totalAmount += itemTotal;
                            totalDiscount += discountAmount;
                            totalItems += item.Value.quantity;
                        }
                    }
                }

                return Json(new
                {
                    success = true,
                    items = cartItems,
                    totalAmount = totalAmount,
                    totalDiscount = totalDiscount,
                    totalItems = totalItems
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            try
            {
                var cartCookie = Request.Cookies["NavinoshoppingCart"];
                if (string.IsNullOrEmpty(cartCookie))
                    return Json(new { success = false, message = "سبد خرید خالی است" });

                var cartData = JsonConvert.DeserializeObject<Dictionary<string, CartItem>>(cartCookie);
                var key = productId.ToString();

                if (cartData != null && cartData.ContainsKey(key))
                {
                    cartData.Remove(key);
                    var updatedCookie = JsonConvert.SerializeObject(cartData);
                    Response.Cookies.Append("NavinoshoppingCart", updatedCookie, new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(7),
                        Path = "/"
                    });

                    return Json(new
                    {
                        success = true,
                        cart = cartData,
                        totalItems = cartData.Sum(x => x.Value.quantity),
                        totalAmount = cartData.Sum(x => {
                            var item = x.Value;
                            var price = (item.PriceAfterOff > 0 && item.PriceAfterOff < item.Price) ? item.PriceAfterOff : item.Price;
                            return price * item.quantity;
                        })
                    });
                }

                return Json(new { success = false, message = "محصول در سبد خرید یافت نشد" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            try
            {
                Response.Cookies.Delete("NavinoshoppingCart", new CookieOptions { Path = "/" });
                return Json(new
                {
                    success = true,
                    cart = new Dictionary<string, CartItem>(),
                    totalItems = 0,
                    totalAmount = 0
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int productId, int change)
        {
            try
            {
                var cartCookie = Request.Cookies["NavinoshoppingCart"];
                if (string.IsNullOrEmpty(cartCookie))
                    return Json(new { success = false, message = "سبد خرید خالی است" });

                var cartData = JsonConvert.DeserializeObject<Dictionary<string, CartItem>>(cartCookie);
                var key = productId.ToString();

                if (cartData == null || !cartData.ContainsKey(key))
                    return Json(new { success = false, message = "محصول در سبد خرید یافت نشد" });

                var newQuantity = cartData[key].quantity + change;

                if (newQuantity <= 0)
                {
                    cartData.Remove(key);
                }
                else
                {
                    var hasStock = await _productSellQueries.ProductSellHaveAmount(productId);
                    if (!hasStock)
                        return Json(new { success = false, message = "موجودی محصول کافی نیست" });

                    cartData[key].quantity = newQuantity;
                }

                var updatedCookie = JsonConvert.SerializeObject(cartData);
                Response.Cookies.Append("NavinoshoppingCart", updatedCookie, new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7),
                    Path = "/"
                });

                return Json(new
                {
                    success = true,
                    cart = cartData,
                    totalItems = cartData.Sum(x => x.Value.quantity),
                    totalAmount = cartData.Sum(x =>
                    {
                        var item = x.Value;
                        var price = (item.PriceAfterOff > 0 && item.PriceAfterOff < item.Price) ? item.PriceAfterOff : item.Price;
                        return price * item.quantity;
                    })
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}