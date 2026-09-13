using Shared;
using Shared.Domain.Enums;
namespace Query.Contract.Admin.Order
{
    public interface IOrderAdminQueryService
    {
        Task<OrdersForAdminPanelPaging> GetOrdersAsync(int orderId, int refId, int pageId, OrderStatus status, string filter="");
        Task<OrderDetailsForAdminQueryModel> GetOrderDetailsForAdminAsync( int OrderId);
    }

    public class OrdersForAdminPanelPaging : BasePaging
    {
        public string Filter { get; set; }
        public string OrderId { get; set; }
       
        public int refId { get; set; }
        public OrderStatus status { get; set; }
        public List<OrderForAdminPanelQueryModel> Orders { get; set; }

    }

    public class OrderDetailsForAdminQueryModel
    {
        public int OrderId { get; set; }
        public OrderPayment OrderPayment { get; set; }
        public int? OrderAddressId { get; set; }
        public OrderStatus status { get; set; }
        public string? DiscountTitle { get; set; }
        public string  OrderDate { get; set; }
        public int DiscountId { get; set; }
        public int DiscountPercent { get; set; }
        public int Price { get; set; }
        public int PriceAfterOff { get; set; }
        public int PaymentPriceSeller { get; set; }
        public int PostPrice { get; set; }
        public int PaymentPrice { get; set; }
        public int DiscountPrice { get; set; }
        public List<OrderSellersAdminQueryModel> OrderSellers { get; set; }
        public OrderAddressAdminQueryModel Address { get; set; }
    }

    public class OrderAddressAdminQueryModel
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
    public class OrderSellersAdminQueryModel
    {
        public int Id { get; set; }
        public int SellerId { get; set; }
        public int SellerCityId { get; set; }
        public string SellerName { get; set; }
        public string? ImageName { get; set; }
        public int DiscountPercent { get; set; }
        public int DiscountId { get; set; }
        public string DiscountTitle { get; set; }
        public int PostPrice { get; set; }
        public string PostTitle { get; set; }
        public int Price { get; set; }
        public int PriceAfterOff { get; set; }
        public int PaymentPrice { get; set; }
        public int DiscountPrice { get; set; }
        public List<OrderItemAdminQueryModel> Items { get; set; }
    }
     public class OrderItemAdminQueryModel
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
}
