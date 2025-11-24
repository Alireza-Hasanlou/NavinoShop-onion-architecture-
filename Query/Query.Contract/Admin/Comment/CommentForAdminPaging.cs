using Shared;
using Shared.Domain.Enums;

namespace Query.Contract.Admin.Comment
{
    public class CommentForAdminPaging : BasePaging
    {
        public List<CommentAdminQueryModel> Comments { get; set; }
        public string? Filter { get; set; }
        public CommentFor For { get; set; }
        public CommentStatus CommentStatus { get; set; }
        public int OwnerId { get; set; }
        public int ? ParentId { get; set; }
        public string? PageTitle { get; set; }
    }
}

