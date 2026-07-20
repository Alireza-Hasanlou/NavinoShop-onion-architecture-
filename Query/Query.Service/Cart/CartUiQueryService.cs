using Discount.Domain.ProductDiscountAgg;
using Microsoft.EntityFrameworkCore;
using Query.Contract.UI.Cart;
using Shop.Domain.CartAgg;
using Shop.Infrastracture.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.User.Agg;

namespace Query.Service.Cart
{
    internal class CartUiQueryService : ICartUiQueryService
    {
        private readonly ShopContext _shopContext;
        private readonly IProductDiscountRepository _productDiscountRepository;

        public CartUiQueryService(ShopContext shopContext, IProductDiscountRepository productDiscountRepository)
        {
            _shopContext = shopContext;
            _productDiscountRepository = productDiscountRepository;
        }

        public async Task<List<CartUiQueryModel>> GetAllAsync(int UserId)
        {
            var carts = await _shopContext.Carts.Where(x => x.UserId == UserId).ToListAsync();
            var productsellIds = carts.Select(x => x.ProductSellId).ToList();
            var CartModel = await _shopContext.productSells.Where(x => productsellIds.Any(p => p == x.Id))
                .Select(x => new CartUiQueryModel
                {
                    ProductSellId = x.Id,
                    ProductId = x.ProductId,
                    Title = x.Product.Title,
                    Seller = x.Seller.Title,
                    Price = x.Price,
                    ImageName = x.Product.ImageName,
                    quantity = 0,
                    Amount = x.Amount,
                    PriceAfterOff = 0

                }).ToListAsync();
            var discounts = await _productDiscountRepository.GetAllBy(x => productsellIds.Any(productsellId =>
            productsellId == x.ProductSellId))
                .ToListAsync();
            foreach (var item in CartModel)
            {
                var cart = carts.FirstOrDefault(x => x.ProductSellId == item.ProductSellId);
                item.quantity = cart.Quantity;
                item.Id = cart.Id;
                var discount = discounts.SingleOrDefault(x => x.ProductSellId == item.ProductSellId && x.EndDate.Date>=DateTime.Now.Date);
                if (discount != null)
                {
                    item.PriceAfterOff = (item.Price * (decimal)discount.Percent) / 100;
                }


            }

            return CartModel;

        }

        public async Task<int> GetCartCountAsync(int userId)
        {
            return await _shopContext.Carts.Where(x => x.UserId == userId).SumAsync(x => x.Quantity);
        }

        public async Task<CartUiQueryModel> GetProductSellForAddToCartById(int productSellId)
        {
            var productsell = await _shopContext.productSells.Where(x => x.Id == productSellId)
                .Select(x => new CartUiQueryModel
                {
                    ProductSellId = x.Id,
                    ProductId = x.ProductId,
                    Title = x.Product.Title,
                    Seller = x.Seller.Title,
                    Price = x.Price,
                    ImageName = x.Product.ImageName,
                    quantity = 0,
                    Amount = x.Amount,
                    PriceAfterOff = 0

                }).SingleOrDefaultAsync();

            var discount = await _productDiscountRepository.GetByProductSellIdAsync(productsell.ProductId, productSellId);
            if (discount != null)
                productsell.PriceAfterOff = productsell.Price * discount.Percent / 100;

            return productsell;
        }
    }
}
