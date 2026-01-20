using Query.Contract.UI.Seo;
using Shared;
using Shared.Ui;

namespace Query.Contract.UI.Blogs
{
    public class BlogsUiQueryPaging : BasePaging
    {
        public string Title { get; set; }
        public string? Filter { get; set; }
        public string? Slug { get; set; }
        public SeoUiQueryModel SeoUi { get; set; }
        public List<BlogsUiQueryModel> Blogs { get; set; }
        public List<BreadCrumb> breadCrumbs { get; set; }
        public List<BlogCategoriesSerchQueryModel> blogCategories { get; set; }
    }
}
