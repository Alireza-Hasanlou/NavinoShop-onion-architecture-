using Shared.Domain;
using Shop.Domain.OrderSellerAgg;
using Shop.Domain.ProductSellAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.OrderItemAgg
{
    public class OrderItem : BaseEntityCreate<int>
    {


        public OrderItem(int orderSellerId, int productSellId, int count, int price, int priceAfterOff)
        {
            OrderSellerId = orderSellerId;
            ProductSellId = productSellId;
            Count = count;
            Price = price;
            PriceAfterOff = priceAfterOff;
        }

        public int OrderSellerId { get; internal set; }
        public int ProductSellId { get; private set; }
        public int Count { get; private set; }
        public int Price { get; private set; }
        public int PriceAfterOff { get; private set; }
        public OrderSeller OrderSeller { get; private set; }
        public ProductSell ProductSell { get; private set; }
        public void Edit(int count, int price, int priceAfterOff)
        {
            Count = count;
            Price = price;
            PriceAfterOff = priceAfterOff;
        }
        public int SumPrice
        {
            get
            {
                return Count * Price;
            }
        }
        public int SumPriceAfterOff
        {
            get
            {
                return Count * PriceAfterOff;
            }
        }
    }
}
