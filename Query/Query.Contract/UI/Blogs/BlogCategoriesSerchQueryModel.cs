namespace Query.Contract.UI.Blog
{
    public class BlogCategoriesSerchQueryModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public int BlogCount { get; set; }
        public List<BlogCategoriesSerchQueryModel> Childs { get; set; }
    }
}
