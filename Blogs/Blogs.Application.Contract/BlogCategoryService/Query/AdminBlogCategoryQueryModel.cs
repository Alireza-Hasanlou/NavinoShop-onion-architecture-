namespace Blogs.Application.Contract.BlogCategoryService.Query
{
    public partial interface IBlogCategoryQueryService
    {
        public class BlogCategoryAdminPageQueryModel
        {
            public int Id { get; set; }
            public string PageTitle { get; set; }
            public List<BlogCategoryAdminQueryModel> Categories { get; set; }
        }

    }
}
