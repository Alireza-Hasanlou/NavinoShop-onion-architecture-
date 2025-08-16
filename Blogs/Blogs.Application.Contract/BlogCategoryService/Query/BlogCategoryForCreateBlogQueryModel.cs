namespace Blogs.Application.Contract.BlogCategoryService.Query
{
    public partial interface IBlogCategoryQueryService
    {
        public class BlogCategoryForCreateBlogQueryModel
        {
            public int Id { get; set; }
            public string Title { get; set; }
        }

    }
}
