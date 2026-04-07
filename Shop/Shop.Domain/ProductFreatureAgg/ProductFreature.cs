
using Shared.Domain;
using Shop.Domain.ProductAgg;

namespace Shop.Domain.ProductFreatureAgg
{
    public class ProductFreature : BaseEntity<int>
    {
        public ProductFreature(int productCategory, string title, string value)
        {
            ProductCategory = productCategory;
            Title = title;
            Value = value;
            Product = new();
        }
        public ProductFreature()
        {

        }

        public int ProductCategory { get; private set; }
        public string Title { get; private set; }
        public string Value { get; private set; }
        public Product Product { get; private set; }

    }

}
