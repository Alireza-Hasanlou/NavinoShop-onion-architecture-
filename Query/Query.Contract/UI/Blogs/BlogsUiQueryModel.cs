namespace Query.Contract.UI.Blogs
{
    public class BlogsUiQueryModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string CreateDate { get; set; }
        public string Writer { get; set; }
        public string Slug { get; set; }
        public int CategoryId { get; set; }
        public string categorySlug { get; set; }
        public string CategoryTitle { get; set; }
        public string ImageName { get; set; }
        public string ImageAlt { get; set; }
    }
}
