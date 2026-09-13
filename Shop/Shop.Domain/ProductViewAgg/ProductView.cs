using Shared.Domain;
using Shop.Domain.ProductAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ProductViewAgg
{
    public class ProductView : BaseEntityCreate<long>
    {
        public ProductView(int userId, int productId, string sessionId)
        {
            UserId = userId;
            ProductId = productId;
            SessionId = sessionId;
        }

        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string SessionId { get; set; }

        public Product Product { get; set; }
    }
}
