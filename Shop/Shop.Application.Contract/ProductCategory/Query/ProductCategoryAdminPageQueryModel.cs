namespace Shop.Application.Contract.ProductCategory.Query
{
    public class ProductCategoryAdminPageQueryModel
    {
        public int ProductCategoryId { get; set; }
        public string Title { get; set; }
        public List<ProductCategoryQueryModel> productCategories { get; set; }
    }
}

