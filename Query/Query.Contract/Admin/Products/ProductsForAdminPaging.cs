using Shared;

namespace Query.Contract.Admin.Products
{
    public class ProductsForAdminPaging:BasePaging
    {
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public string Filter { get; set; }
        public List<ProductsForAdminQueryModel> products { get; set; }
    }
}
