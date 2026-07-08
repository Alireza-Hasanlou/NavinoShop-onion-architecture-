using Shared.Application;
using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Contract.Order.Command
{
    public interface IOrderCommands
    {
        Task<OperationResultOrderDiscount> ApplySellerDiscountAsync(int userId, int sellerId, int DiscountId, int DiscountPercent,string Title);
        Task<OperationResult> UpsertUserOrder(int UserId , List <ShopCartViewModel> cart); 
    }
    public class ShopCartViewModel
    {
        public int Id { get; set; }
        public int ProductSellId { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public int Price { get; set; }
        public int PriceAfterOff { get; set; }
        public string Seller { get; set; }
        public int Quantity { get; set; }
        public int Amount { get; set; }
      
    }

}
