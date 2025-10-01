namespace Comments.Application.Contract.CommentService.Command
{
    public class RejectCommentCommandModel
    {
        public long Id { get; set; }
        public string Why { get; set; }
    }
}
