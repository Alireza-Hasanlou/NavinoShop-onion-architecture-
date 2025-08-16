namespace Blogs.Application.Contract.BlogApplication.Command
{
    public class EditBlogCommand:CreateBlogCommand
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
    }
}
