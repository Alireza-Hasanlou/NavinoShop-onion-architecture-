

namespace Shop.Application.Contract.ProductCategory.Query
{
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

