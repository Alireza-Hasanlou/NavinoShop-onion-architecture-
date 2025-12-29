namespace Blogs.Application.Contract.BlogService.Query
{
    public class LastBlogsQueryModel
    {
        public LastBlogsQueryModel(string title, string slug)
        {
            Title = title;
            Slug = slug;
        }

        public string Title { get; private set; }
        public string Slug { get; private set; }

    }
}
