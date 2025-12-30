namespace Blogs.Application.Contract.BlogService.Query
{
    public class MostViewdBlogsForIndexQueryModel
    {
        public int CategoryId { get; set; }
        public string CategorySlug { get; set; }
        public string CategoryTitle { get; set; }
        public List<BestBlogQueryModel> BestBlogQueryModels { get; set; }
    }
}
