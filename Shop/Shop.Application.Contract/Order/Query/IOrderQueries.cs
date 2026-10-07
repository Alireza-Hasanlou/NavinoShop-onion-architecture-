using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Contract.Order.Query
{
    public interface IOrderQueries
    {
        Task<FactorforordercancellationQueryModel> GetFactorforordercancellation(int OrderId , int SellerId);
    }
    public class FactorforordercancellationQueryModel
    {
        public int  OrderId { get; set; }
        public int PaymentPrice { get; set; }
        public int SellerId { get; set; }
        public int CustomerId { get; set; }
        public List<SellersProductsQueryModel> Products { get; set; } = new();

    }

    public class SellersProductsQueryModel
    {
        public int  ProductSellId { get; set; }
        public int Count { get; set; }
       
    }
}
