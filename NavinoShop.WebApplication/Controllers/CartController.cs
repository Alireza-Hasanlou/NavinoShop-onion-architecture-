using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NavinoShop.WebApplication.Services;
using NavinoShop.WebApplication.Utility.ViewModels;
using Newtonsoft.Json;
using Query.Contract.UI.Cart;
using Shared.Application;
using Shared.Application.Auth;
using Shop.Application.Contract.Cart;
using Shop.Application.Contract.ProductSell.Query;
using Shop.Domain.ProductSellAgg;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Controllers
{
    [IgnoreAntiforgeryToken]

    public class CartController : Controller
    {
        private readonly IProductSellQueries _productSellQueries;
        private readonly ICartUiQueryService _cartUiQueryService;
        private readonly IAuthService _authService;
        private readonly ICartCommands _cartCommands;

        public CartController(IProductSellQueries productSellQueries, ICartUiQueryService cartUiQueryService,
            IAuthService authService, ICartCommands cartCommands)
        {
            _productSellQueries = productSellQueries;
            _cartUiQueryService = cartUiQueryService;
            _authService = authService;
            _cartCommands = cartCommands;
        }

        [HttpGet]
        [Route("/Cart")]
        public async Task<IActionResult> Index()
        {
            var cartItems = new List<CartItemViewModel>();

            if (_authService.IsUserLogin())
            {
                var userId = _authService.GetLoginUserId();
                var carts = await _cartUiQueryService.GetAllAsync(userId);

                foreach (var item in carts)
                {
                    cartItems.Add(new CartItemViewModel
                    {
                        Id = item.ProductSellId,
                        Title = item.Title ?? "بدون عنوان",
                        ImageName = FileDirectories.ProductImageDirectory100 + item.ImageName,
                        Price = item.Price,
                        PriceAfterOff = item.PriceAfterOff,
                        Seller = item.Seller ?? "نام فروشنده",
                        Quantity = item.quantity,
                        Amount = item.Amount,
                        TotalPrice = (item.PriceAfterOff > 0 && item.PriceAfterOff < item.Price)
                            ? item.PriceAfterOff * item.quantity
                            : item.Price * item.quantity
                    });
                }
            }
            else
            {
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
                                    ImageName = FileDirectories.ProductImageDirectory100 + item.Value.ImageName,
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
                        return View(new CartViewModel());
                    }
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
            if (_authService.IsUserLogin())
            {
                var userId = _authService.GetLoginUserId();
                var carts = await _cartUiQueryService.GetAllAsync(userId);

                decimal totalAmount = 0;
                decimal totalDiscount = 0;
                int totalItems = 0;
                var cartItems = new List<object>();

                foreach (var item in carts)
                {
                    var hasDiscount = item.PriceAfterOff > 0 && item.PriceAfterOff < item.Price;
                    var finalPrice = hasDiscount ? item.PriceAfterOff : item.Price;
                    var itemTotal = finalPrice * item.quantity;
                    var discountAmount = hasDiscount ? (item.Price - item.PriceAfterOff) * item.quantity : 0;
                    var hasStock = await _productSellQueries.ProductSellHaveAmount(item.ProductSellId , item.quantity);

                    cartItems.Add(new
                    {
                        id = item.ProductSellId,
                        title = item.Title ?? "بدون عنوان",
                        imageName = FileDirectories.ProductImageDirectory100 + item.ImageName,
                        price = item.Price,
                        priceAfterOff = item.PriceAfterOff,
                        seller = item.Seller ?? "نام فروشنده",
                        quantity = item.quantity,
                        amount = item.Amount,
                        totalPrice = itemTotal,
                        hasDiscount = hasDiscount,
                        hasStock = hasStock
                    });

                    totalAmount += itemTotal;
                    totalDiscount += discountAmount;
                    totalItems += item.quantity;
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
            else
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
                                var hasStock = await _productSellQueries.ProductSellHaveAmount(int.Parse(item.Key),item.Value.quantity);

                                cartItems.Add(new
                                {
                                    id = int.Parse(item.Key),
                                    title = item.Value.Title ?? "بدون عنوان",
                                    imageName = FileDirectories.ProductImageDirectory100 + item.Value.ImageName,
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
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productSellId, int quantity = 1)
        {
            try
            {
                var haveAmount = await _productSellQueries.ProductSellHaveAmount(productSellId, quantity);
                if(!haveAmount)
                    return Json(new
                    {
                        success = false,

                        message = "مثل اینکه موجودی محصول تو انبار تموم شده"
                    });



                if (_authService.IsUserLogin())
                {
                    var userId = _authService.GetLoginUserId();
                    var res = await _cartCommands.CreateAsync(userId, productSellId, quantity);


                    return Json(new
                    {
                        success = res.Success,

                        message = res.Message
                    });
                }
                else
                {
                    var cartCookie = Request.Cookies["NavinoshoppingCart"];
                    Dictionary<string, CartItem> cartData;

                    if (!string.IsNullOrEmpty(cartCookie))
                    {
                        cartData = JsonConvert.DeserializeObject<Dictionary<string, CartItem>>(cartCookie);
                    }
                    else
                    {
                        cartData = new Dictionary<string, CartItem>();
                    }

                    var key = productSellId.ToString();

                    if (cartData.ContainsKey(key))
                    {
                        cartData[key].quantity += quantity;
                    }
                    else
                    {
                        var product = await _cartUiQueryService.GetProductSellForAddToCartById(productSellId);
                        cartData[key] = new CartItem
                        {
                            Id = key,
                            Title = product.Title,
                            ImageName = product.ImageName,
                            Price = product.Price,
                            PriceAfterOff = product.PriceAfterOff,
                            Seller = product.Seller,
                            quantity = quantity,
                            Amount = product.Amount
                        };
                    }

                    var updatedCookie = JsonConvert.SerializeObject(cartData);
                    Response.Cookies.Append("NavinoshoppingCart", updatedCookie, new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(7),
                        Path = "/"
                    });

                    var totalItems = cartData.Sum(x => x.Value.quantity);

                    return Json(new
                    {
                        success = true,
                        cartCount = totalItems,
                        message = "محصول با موفقیت به سبد خرید اضافه شد"
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            try
            {
                if (_authService.IsUserLogin())
                {
                    var userId = _authService.GetLoginUserId();
                    await _cartCommands.DeleteAsync(userId, productId);


                    return Json(new
                    {
                        success = true,

                        message = "محصول با موفقیت از سبد خرید حذف شد"
                    });
                }
                else
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

                        var totalItems = cartData.Sum(x => x.Value.quantity);
                        var totalAmount = cartData.Sum(x =>
                        {
                            var item = x.Value;
                            var price = (item.PriceAfterOff > 0 && item.PriceAfterOff < item.Price) ? item.PriceAfterOff : item.Price;
                            return price * item.quantity;
                        });

                        return Json(new
                        {
                            success = true,
                            cartCount = totalItems,
                            cart = cartData,
                            totalItems = totalItems,
                            totalAmount = totalAmount,
                            message = "محصول با موفقیت از سبد خرید حذف شد"
                        });
                    }

                    return Json(new { success = false, message = "محصول در سبد خرید یافت نشد" });
                }
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
                if (_authService.IsUserLogin())
                {
                    var userId = _authService.GetLoginUserId();
                    var UpdateResult = await _cartCommands.UpdateQuantityAsync(userId, productId, change);


                    if (UpdateResult.Success)
                    {
                        return Json(new
                        {
                            success = true,

                            message = "تعداد محصول با موفقیت به‌روزرسانی شد"
                        });

                    }
                    else
                    {
                        return Json(new
                        {
                            success = false,

                            message = UpdateResult.Message
                        });
                    }

                }
                else
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
                        var hasStock = await _productSellQueries.ProductSellHaveAmount(productId , newQuantity);
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

                    var totalItems = cartData.Sum(x => x.Value.quantity);
                    var totalAmount = cartData.Sum(x =>
                    {
                        var item = x.Value;
                        var price = (item.PriceAfterOff > 0 && item.PriceAfterOff < item.Price) ? item.PriceAfterOff : item.Price;
                        return price * item.quantity;
                    });

                    return Json(new
                    {
                        success = true,
                        cartCount = totalItems,
                        cart = cartData,
                        totalItems = totalItems,
                        totalAmount = totalAmount,
                        message = "تعداد محصول با موفقیت به‌روزرسانی شد"
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                if (_authService.IsUserLogin())
                {
                    var userId = _authService.GetLoginUserId();
                    await _cartCommands.ClearCartAsync(userId);

                    return Json(new
                    {
                        success = true,
                        cartCount = 0,
                        message = "سبد خرید با موفقیت خالی شد"
                    });
                }
                else
                {
                    Response.Cookies.Delete("NavinoshoppingCart", new CookieOptions { Path = "/" });
                    return Json(new
                    {
                        success = true,
                        cartCount = 0,
                        cart = new Dictionary<string, CartItem>(),
                        totalItems = 0,
                        totalAmount = 0,
                        message = "سبد خرید با موفقیت خالی شد"
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCartCount()
        {
            try
            {
                // بررسی لاگین بودن کاربر
                if (_authService.IsUserLogin())
                {
                    var userId = _authService.GetLoginUserId();
                    var cartcount = await _cartUiQueryService.GetCartCountAsync(userId);
                    return Json(new
                    {
                        success = true,
                        cartCount = cartcount,
                        isLoggedIn = true
                    });
                }
                else
                {
                    // دریافت از کوکی
                    var cartCookie = Request.Cookies["NavinoshoppingCart"];
                    int cartCount = 0;

                    if (!string.IsNullOrEmpty(cartCookie))
                    {
                        try
                        {
                            var cartData = JsonConvert.DeserializeObject<Dictionary<string, CartItem>>(cartCookie);
                            if (cartData != null && cartData.Any())
                            {
                                cartCount = cartData.Sum(x => x.Value.quantity);
                            }
                        }
                        catch (Exception ex)
                        {
                            // در صورت خطا در دسریالایز، تعداد را صفر در نظر بگیر
                            Console.WriteLine($"Error deserializing cart cookie: {ex.Message}");
                        }
                    }

                    return Json(new
                    {
                        success = true,
                        cartCount = cartCount,
                        isLoggedIn = false
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message,
                    cartCount = 0
                });
            }
        }

        private string GetSessionId()
        {
            var sessionId = Request.Cookies["CartSessionId"];
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = Guid.NewGuid().ToString();
                Response.Cookies.Append("CartSessionId", sessionId, new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Path = "/"
                });
            }
            return sessionId;
        }

        [HttpPost]
        public async Task<IActionResult> SyncCartFromCookie()
        {
            try
            {

                if (!_authService.IsUserLogin())
                    return Json(new { success = false, message = "کاربر لاگین نیست" });

                var userId = _authService.GetLoginUserId();
                var cartCookie = Request.Cookies["NavinoshoppingCart"];

                if (string.IsNullOrEmpty(cartCookie))
                {
                    var cartCount = await _cartUiQueryService.GetCartCountAsync(userId);
                    return Json(new
                    {
                        success = true,
                        message = "سبد خرید کوکی خالی است",
                        cartCount = cartCount,
                        syncedItems = 0
                    });
                }

                var cartData = JsonConvert.DeserializeObject<Dictionary<string, CartItem>>(cartCookie);
                if (cartData == null || !cartData.Any())
                {
                    var cartCount = await _cartUiQueryService.GetCartCountAsync(userId);
                    return Json(new
                    {
                        success = true,
                        message = "سبد خرید کوکی خالی است",
                        cartCount = cartCount,
                        syncedItems = 0
                    });
                }


                var dbCartItems = await _cartUiQueryService.GetAllAsync(userId);
                var dbCartDict = dbCartItems.ToDictionary(x => x.ProductSellId);

                int addedCount = 0;
                int updatedCount = 0;
                int skippedCount = 0;
                bool anyChanges = false;

                foreach (var cookieItem in cartData)
                {
                    var productSellId = int.Parse(cookieItem.Key);
                    var cookieQuantity = cookieItem.Value.quantity;


                    var hasStock = await _productSellQueries.ProductSellHaveAmount(productSellId,cookieQuantity);
                    if (!hasStock)
                    {

                        anyChanges = true;
                        skippedCount++;
                        continue;
                    }


                    if (dbCartDict.TryGetValue(productSellId, out var dbItem))
                    {

                        int dbQuantity = dbItem.quantity;
                        int dbAmount = dbItem.Amount;


                        if (cookieQuantity != dbQuantity)
                        {

                            if (cookieQuantity > dbQuantity)
                            {
                                int maxAvailable = dbAmount;
                                int canAdd = Math.Min(cookieQuantity - dbQuantity, maxAvailable - dbQuantity);  //  2-4      10  4   

                                if (canAdd > 0)
                                {

                                    var updateResult = await _cartCommands.UpdateQuantityAsync(userId, productSellId, canAdd);
                                    if (updateResult.Success)
                                    {
                                        updatedCount++;
                                        anyChanges = true;
                                    }
                                }
                                else if (canAdd == 0 && cookieQuantity > dbQuantity)
                                {

                                    cookieItem.Value.quantity = dbAmount;
                                    anyChanges = true;
                                }
                            }

                        }

                    }
                    else
                    {

                        await _cartCommands.CreateAsync(userId, productSellId, cookieQuantity);
                        addedCount++;
                        anyChanges = true;
                    }
                }


                Response.Cookies.Delete("NavinoshoppingCart", new CookieOptions { Path = "/" });


                var newCartCount = await _cartUiQueryService.GetCartCountAsync(userId);

                return Json(new
                {
                    success = true,
                    message = "سبد خرید با موفقیت همگام‌سازی شد",
                    cartCount = newCartCount,
                    addedCount = addedCount,
                    updatedCount = updatedCount,
                    skippedCount = skippedCount,
                    hasChanges = anyChanges,
                    syncedItems = addedCount + updatedCount
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = $"خطا در همگام‌سازی: {ex.Message}"
                });
            }
        }
    }
}