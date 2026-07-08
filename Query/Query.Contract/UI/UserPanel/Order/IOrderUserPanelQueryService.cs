using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Contract.UI.UserPanel.Order
{
    public interface IOrderUserPanelQueryService
    {
        Task<OrderUserPanelViewModel> GetOrderAsync(int userId);
    }
    public class OrderUserPanelViewModel
    {
        public int OrderId { get; set; }
        public OrderPayment OrderPayment { get; set; }
        public int? OrderAddressId { get; set; }
        public string? DiscountTitle { get; set; }
        public int DiscountId { get; set; }
        public int DiscountPercent { get; set; }
        public int Price { get; set; }
        public int PriceAfterOff { get; set; }
        public int PaymentPriceSeller { get; set; }
        public int PostPrice { get; set; }
        public int PaymentPrice { get; set; }
        public int DiscountPrice { get; set; }
        public List<OrderSellerUserPanelQueryModel> OrderSellers { get; set; }
        public List<OrderAddressQueryModel> Addresses { get; set; }

    }

    public class OrderSellerUserPanelQueryModel
    {
        public int Id { get; set; }
        public int SellerId { get; set; }
        public string SellerName { get; set; }
        public int DiscountPercent { get; set; }
        public int DiscountId { get; set; }
        public string DiscountTitle { get; set; }
        public int PostPrice { get; set; }
        public int Price { get; set; }
        public int PriceAfterOff { get; set; }
        public int PaymentPrice { get; set; }
        public int DiscountPrice { get; set; }
        public List<OrderItemQueryMoedel> Items { get; set; }

    }
    public class OrderItemQueryMoedel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ProductSellId { get; set; }
        public string ProductName { get; set; }
        public int SellerId { get; set; }
        public int Price { get; set; }
        public int PriceAfterOff { get; set; }
        public string ImageName { get; set; }
        public string ImageAlt { get; set; }
        public int Quantity { get; set; }

    }

    public class OrderAddressQueryModel
    {

        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string AddressDetail { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }
        public string? NationalCode { get; set; }
    }
}