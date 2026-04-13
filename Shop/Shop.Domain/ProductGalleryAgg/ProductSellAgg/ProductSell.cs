using Shared.Domain;
using Shared.Domain.Enums;
using Shop.Domain.OrderItemAgg;
using Shop.Domain.ProductAgg;
using Shop.Domain.SellerAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ProductSellAgg
{
    // Connect a Product to Seller
    public class ProductSell : BaseEntityCreateUpdateActive<int>
    {
        public ProductSell()
        {
            Product = new();
            Seller = new();
            OrderItems = new List<OrderItem>();
        }
        public ProductSell(int productId, int price, string unit, int sellerId, int weight)
        {
            ProductId = productId;
            Price = price;
            Unit = unit;
            SellerId = sellerId;
            Amount = 0;
            Weight = weight;
            SetActivation(false);
        }

        public int ProductId { get; private set; }
        public int Price { get; private set; }
        public int Amount { get; private set; }
        public string Unit { get; private set; }
        public int SellerId { get; private set; }
        public int Weight { get; private set; }

        public Product Product { get; private set; }
        public Seller Seller { get; private set; }
        public ICollection<OrderItem> OrderItems { get; private set; }

        public void Edit(int price, string unit, int weight)
        {
            Price = price;
            Unit = unit;
            Weight = weight;
            SetActivation(false);
        }
        public void ChangeAmount(int amount, StoreProductType type)
        {
            switch (type)
            {
                case StoreProductType.افزایش:
                    Amount = Amount + amount;
                    break;
                case StoreProductType.کاهش:
                    Amount = Amount - amount < 0 ? 0 : Amount - amount;
                    break;
                default:
                    break;
            }
        }
    }
}
