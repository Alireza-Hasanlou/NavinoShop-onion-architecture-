using Shared.Domain.Enums;

namespace Query.Contract.Admin.Comment
{
    public class CommentAdminQueryModel
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string CommentTitle { get; set; }
        public int OwnerId { get; set; }
        public CommentFor For { get; set; }
        public CommentStatus CommentStatus { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? WhyRejected { get; set; }
        public string? Text { get; set; }
        public bool HaveChild { get; set; }
        public long? ParentId { get; set; }
    }
}
