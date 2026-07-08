using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Contract.UI.Cart
{
    public interface ICartUiQueryService
    {
        Task<List<CartUiQueryModel>> GetAllAsync(int UserId);
        Task<int> GetCartCountAsync(int userId);
        Task<CartUiQueryModel> GetProductSellForAddToCartById(int productSellId);
    }

    public class CartUiQueryModel
    {
        public int Id { get; set; }
        public int  ProductId { get; set; }
        public int ProductSellId { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public int Price { get; set; }
        public int quantity { get; set; }
        public decimal PriceAfterOff { get; set; }
        public string Seller { get; set; }
        public int Amount { get; set; }
    }
}
