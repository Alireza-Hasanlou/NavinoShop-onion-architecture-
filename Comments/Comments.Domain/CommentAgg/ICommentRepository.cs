using Shared.Domain;

namespace Comments.Domain.CommentAgg
{
    public interface ICommentRepository : IGenericRepository<Comment, long> { }
}
