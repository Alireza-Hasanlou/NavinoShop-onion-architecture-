using Shared;
using Shared.Domain.Enums;

namespace Query.Contract.UI.Comments
{
    public class CommentsUiPaging:BasePaging
    {
        public int OwnerId { get; set; }
        public CommentFor CommentFor{ get; set; }
        public List<CommentUiQueryModel> Comments { get; set; }
    }
}
