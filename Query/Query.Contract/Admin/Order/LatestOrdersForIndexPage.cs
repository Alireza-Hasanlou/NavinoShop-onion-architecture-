using Shared.Domain.Enums;
using System.Text.Json;
namespace Query.Contract.Admin.Order
{
    public class LatestOrdersForIndexPage
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateAt { get; set; }
        public string CustomerName { get; set; }
        public int Price { get; set; }
        public OrderStatus Status { get; set; }
    }
  
}
