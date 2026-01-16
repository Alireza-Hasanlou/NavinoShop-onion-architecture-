
using Shared.Application;
namespace Comments.Application.Contract.CommentService.Command
{
    public interface ICommentCommandService
    {
        Task<OperationResult> Create(CreateCommentCommandModel command);
        Task<OperationResult> Reject(RejectCommentCommandModel command);
        Task<OperationResult> AcceptedComment(long id);
        Task<OperationResult> DeleteComment(long id);
    }
}
