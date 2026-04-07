using Shared.Domain;
using Shop.Domain.OrderItemAgg;
using Shop.Domain.ProductAgg;
using Shop.Domain.SellerAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Product_SellerAgg
{
    // Connect a Product to Seller
    public class Product_Seller : BaseEntityCreateUpdateActive<int>
    {
        public Product_Seller()
        {
            Product = new();
            Seller = new();
            OrderItems = new List<OrderItem>();
        }
        public Product_Seller(int productId, int price, int unit, int sellerId,int weight)
        {
            ProductId = productId;
            Price = price;
            Unit = unit;
            SellerId = sellerId;
            Weight = weight;
            SetActivation(false);
        }

        public int ProductId { get; private set; }
        public int Price { get; private set; }
        public int Unit{ get; private set; }
        public int SellerId { get; private set; }
        public int Weight { get; private set; }

        public Product Product { get; private set; }
        public Seller Seller { get; private set; }
        public ICollection<OrderItem> OrderItems { get; private set; }

        public void Edit( int price, int unit,int wight)
        {
            Price = price;
            Unit = unit;
            Weight= wight;
            SetActivation(false);
        }

    }
}
