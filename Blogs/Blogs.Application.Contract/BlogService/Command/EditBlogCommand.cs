namespace Blogs.Application.Contract.BlogService.Command
{
    public class EditBlogCommand:CreateBlogCommand
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
    }
}
