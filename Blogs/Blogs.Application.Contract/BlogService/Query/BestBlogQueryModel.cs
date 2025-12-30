namespace Blogs.Application.Contract.BlogService.Query
{
    public class BestBlogQueryModel
    {
       
        public string Title { get; set; }
        public string Writer { get; set; }
        public string CreateDate { get; set; }
        public string ImageAlt { get; set; }
        public string ImageName { get; set; }
        public string BlogSlug { get; set; }
        public string ShortDescription { get; set; }

    }
}
