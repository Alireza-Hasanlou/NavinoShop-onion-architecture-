using Shared.Application;
using Shared.Application.Validations;
using Shop.Application.Contract.Cart;
using Shop.Domain.CartAgg;
using Shop.Domain.ProductSellAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Commands
{
    internal class CartCommands : ICartCommands
    {
        private readonly ICartRepository _cartRepository;
        public CartCommands(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<OperationResult> ClearCartAsync(int userId)
        {
            var result = await _cartRepository.ClearCartAsync(userId);
            if (result)
                return new(true);
            return new(false, ValidationMessages.SystemErrorMessage);
        }

        public async Task<OperationResult> CreateAsync(int UserId, int ProductSellId, int Quantity)
        {
            if (UserId < 1 || ProductSellId < 1)
                return new OperationResult(false, "داده های نامعتبر");
            var cartProduct = await _cartRepository.GetCartProductAsync(UserId, ProductSellId);
            if (cartProduct != null)
            {
                cartProduct.ChangeQuantity(1);
                if (await _cartRepository.SaveAsync())
                    return new(true);
                return new(false, ValidationMessages.SystemErrorMessage);
            }
            else
            {
                Cart cart = new(ProductSellId, UserId, Quantity);
                var Result = await _cartRepository.CreateAsync(cart);
                if (Result.Success)
                    return new(true);
                return new(false, ValidationMessages.SystemErrorMessage);
            }
        }

        public async Task<OperationResult> DeleteAsync(int uesrId, int productId)
        {
            var cart = await _cartRepository.GetCartProductAsync(uesrId, productId);
            if (cart == null)
                return new(false, "داده های نامعتبر");
            var Result = await _cartRepository.DeleteAsync(cart);
            if (Result.Success)
                return new(true);
            return new(false, ValidationMessages.SystemErrorMessage);
        }

        public async Task<OperationResult> UpdateQuantityAsync(int userId, int productId, int change)
        {
            var cartProduct = await _cartRepository.GetCartProductAsync(userId, productId);
            cartProduct.ChangeQuantity(change);
            if (await _cartRepository.SaveAsync())
                return new(true);
            return new(false, ValidationMessages.SystemErrorMessage);
        }
    }
}
