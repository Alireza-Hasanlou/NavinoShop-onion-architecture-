namespace Blogs.Application.Contract.BlogCategoryService.Query
{
  
    
        public class BlogCategoryAdminPageQueryModel
        {
            public int parentId { get; set; }
            public string PageTitle { get; set; }
            public List<BlogCategoryAdminQueryModel> Categories { get; set; }
        }

    
}
