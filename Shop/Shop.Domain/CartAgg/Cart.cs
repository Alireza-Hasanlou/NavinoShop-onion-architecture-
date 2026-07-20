using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.CartAgg
{
    public class Cart : BaseEntityCreate<int>
    {
        public Cart(int productSellId, int userId, int quantity)
        {
            ProductSellId = productSellId;
            UserId = userId;
            Quantity = quantity;
        }

        public int ProductSellId { get; private set; }
        public int UserId { get; private set; }
        public int Quantity { get; private set; }

        
        public void ChangeQuantity(int quantity)
        {
            Quantity = Quantity + quantity;
            if (Quantity < 1)
                Quantity = 1;
        }

    }
}
