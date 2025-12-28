namespace Blogs.Application.Contract.BlogService.Query
{
    public class LastBlogsQueryModel
    {
        public LastBlogsQueryModel(string title, string url)
        {
            Title = title;
            Url = url;
        }

        public string Title { get; private set; }
        public string Url { get; private set; }

    }
}
