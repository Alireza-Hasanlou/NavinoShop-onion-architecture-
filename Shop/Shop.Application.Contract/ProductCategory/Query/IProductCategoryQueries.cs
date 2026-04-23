

namespace Shop.Application.Contract.ProductCategory.Query
{
    public interface IProductCategoryQueries
    {
        Task<bool> CheckCategoryHaveParent(int id);
        Task<ProductCategoryAdminPageQueryModel> GetCategoriesForAdmin(int id);
        Task<List<CategoryTreeItem>> GetCategoriesForAddProduct();
        Task<List<ProductCategoryForAddProductSeller>> GetCategoryForAddProductSells(int id);
    }
    public class CategoryTreeItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ParentId { get; set; }
        public int Level { get; set; }
        public bool IsChecked { get; set; }
        public bool HasChildren { get; set; }
        public List<CategoryTreeItem> Children { get; set; } = new List<CategoryTreeItem>();
    }
}

