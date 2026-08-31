using Shared.Application;
using Shared.Domain;
using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Contract.Order.Command
{
    public interface IOrderCommands
    {
        Task<PricesAfterApplyDiscountDto> ApplySellerDiscountAsync(int userId, int sellerId, int DiscountId, int DiscountPercent, string Title);
        Task<PricesAfterApplyDiscountDto> RemoveSellerDiscountAsync(int UserId, int sellerId);
        Task<PricesAfterApplyDiscountDto> ApplyOrderDiscountAsync(int userId, int DiscountId, int DiscountPercent, string Title);
        Task<OperationResult> UpsertUserOrder(int UserId, List<ShopCartViewModel> cart , UpsertOrderAddressCommandModel userAddress );
        Task<PricesAfterApplyDiscountDto> RemoveOrderDiscountAsync(int userId, int orderId);
        Task<OperationResult> UpsertOrderAddressAsync(int UserId, UpsertOrderAddressCommandModel command);
        Task<OperationResult> SetOrderPaymentType(OrderPayment orderPayment, int userId);
        Task<OperationResult> FinalizePaymentAsync(int UserId);


    }

    public class UpsertOrderAddressCommandModel
    {
        public int StateId { get; private set; }
        public int CityId { get; private set; }
        public string AddressDetail { get; private set; }
        public string PostalCode { get; private set; }
        public string Phone { get; private set; }
        public string FullName { get; private set; }
        public string NationalCode { get; private set; }

      
    }
         
    public class PricesAfterApplyDiscountDto
    {


        public bool Success { get; set; }
        public string Message { get; set; }
        public decimal DiscountPrice { get; set; }
        public int sellerId { get; set; }
        public int SellerPrice { get; set; }
        public int SellerPaymentPrice { get; set; }
        public int DiscountId  { get; set; }
        public int SellerPriceAfterOff { get; set; }
        public int DiscountPercent { get; set; }
        public string DiscountTitle { get; set; }
        public int PaymentPrice { get; set; }
        public int TolalPrice { get; set; }
        public int totlaPriceAfterOff { get; set; }
    }
}