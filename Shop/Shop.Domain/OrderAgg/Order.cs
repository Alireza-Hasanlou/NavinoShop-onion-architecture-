using Shared.Domain;
using Shared.Domain.Enums;
using Shop.Domain.OrderAddressAgg;
using Shop.Domain.OrderItemAgg;
using Shop.Domain.OrderSellerAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.OrderAgg
{
    public class Order : BaseEntityCreateUpdate<int>
    {
        public int UserId { get; private set; }
        public OrderStatus OrderStatus { get; private set; }
        public OrderPayment OrderPayment { get; private set; }
        public int OrderAddressId { get; private set; }
        public int DiscountId { get; private set; }
        public int DiscountPercent { get; private set; }
        public string DiscountTitle { get; private set; }
        public ICollection<OrderSeller> OrderSellers { get; private set; }
        public OrderAddress OrderAddress { get; private set; }
        public int Price
        {
            get
            {
                return OrderSellers.Sum(o => o.Price);
            }
        }
        public int PriceAfterOff
        {
            get
            {
                return OrderSellers.Sum(o => o.PriceAfterOff);
            }
        }
        public int PaymentPriceSeller
        {
            get
            {
                return OrderSellers.Sum(o => o.PaymentPrice);
            }
        }
        public int PostPrice
        {
            get
            {
                return OrderSellers.Sum(o => o.PostPrice);
            }
        }
        public int PaymentPrice
        {
            get
            {
                var discountPrice = DiscountPercent * PaymentPriceSeller / 100;


                return PaymentPriceSeller - discountPrice + PostPrice;
            }
        }
        public Order()
        {
            OrderSellers = new List<OrderSeller>();
            OrderAddress = new();
        }
        public Order(int userId)
        {
            UserId = userId;
            OrderStatus = OrderStatus.پرداخت_نشده;
            OrderPayment = OrderPayment.پرداخت_از_درگاه;
            OrderAddressId = 0;
            DiscountId = 0;
            DiscountPercent = 0;
        }
        public void ChangeStatus(OrderStatus status)
        {
            OrderStatus = status;
            UpdateEntity();
        }
        public void ChangePayment(OrderPayment payment)
        {
            OrderPayment = payment;
        }
        public void AddAddress(int addressId)
        {
            OrderAddressId = addressId;
            UpdateEntity();
        }
        public void AddDiscount(int discountId, int percent, string title)
        {
            DiscountId = discountId;
            DiscountPercent = percent;
            DiscountTitle = title;
            UpdateEntity();
        }
        public void AddOrderSeller(OrderSeller seller)
        {
            seller.OrderId = Id;
            OrderSellers.Add(seller);
        }

        public void ChangeAddress(int key)
        {
            OrderAddressId = key;
        }
    }
}

