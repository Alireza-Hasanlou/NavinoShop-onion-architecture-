namespace Query.Contract.UI.Comments
{
    public class CommentUiQueryModel
    {
        public long Id { get; set; }
        public int OwnerId { get; set; }
        public long?  ParentId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string CreateDate { get; set; }
        public string Text { get; set; }
        public string ImageName { get; set; }

        public List<CommentUiQueryModel> Replys { get; set; }
    }
}
