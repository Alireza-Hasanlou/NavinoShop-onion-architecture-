using Shared;

namespace Shop.Application.Contract.Product.Query
{
    public class ProductsForAdminPaging : BasePaging
    {
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public string Filter { get; set; }
        public int PageId { get; set; }
        public int Take { get; set; }
        public List<ProductsForAdminQueryModel> products { get; set; }
    }
}
