using Shared.Domain;
using Utility.Shared.Domain;

namespace Comments.Domain.CommentAgg
{
    public interface ICommentRepository : IGenericRepository<Comment, long> { }
}
