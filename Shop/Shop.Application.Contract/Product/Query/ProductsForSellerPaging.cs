using Shared;

namespace Shop.Application.Contract.Product.Query
{
    public class ProductsForSellerPaging : BasePaging
    {
        public int SellerId { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public string Filter { get; set; }
        public List<ProductsForSellerQueryModel> products { get; set; }
    }
}
