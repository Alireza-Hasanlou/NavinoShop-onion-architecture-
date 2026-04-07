
using Shared.Domain;
using Shop.Domain.ProductAgg;

namespace Shop.Domain.ProductFreatureAgg
{
    public class ProductFreature : BaseEntity<int>
    {
        public ProductFreature(int productId, string title, string value)
        {
            ProductId = productId;
            Title = title;
            Value = value;

        }
        public ProductFreature()
        {
            Product = new();
        }

        public int ProductId { get; private set; }
        public string Title { get; private set; }
        public string Value { get; private set; }
        public Product Product { get; private set; }

        public void Edit( string title, string value)
        {
         
            Title = title;
            Value = value;

        }
    }

}
