using Shared.Domain;
using Store.Domain.StoreProductAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.StoreAgg
{
    public class Store : BaseEntityCreate<int>
    {
        public Store( int sellerId, string? description, int userId)
        {
            SellerId = sellerId;
            Description = description;
            UserId = userId;
        }
        public Store()
        {
            StoreProducts = new List<StoreProduct>();
        }
        public int UserId { get; set; }
        public int SellerId { get; private set; }
        public string? Description { get; private set; }
        public ICollection<StoreProduct> StoreProducts { get; private set; }
        public void EditDescription(string description)
        {
            Description = description;
        }
    }
}
