using Query.Contract.UI.Comments;

namespace Query.Contract.UI.Blog
{
    public class SingleBlogQueryModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Writer { get; set; }
        public string CreateDate { get; set; }
        public int VisitCount { get; set; }
        public int CommentCount { get; set; }
        public int CategoryId { get; set; }
        public string CategorySlug { get; set; }
        public string CategoryTitle { get; set; }
        public string ImageName { get; set; }
        public string ImageAlt { get; set; }
        public List<BlogsUiQueryModel> RelateBlogs { get; set; }
        public List<CommentUiQueryModel> Comments { get; set; }
        public List<BreadCrumb> BreadCrumbs { get; set; }
        public SeoUiQueryModel Seo { get; set; }

    }

}
