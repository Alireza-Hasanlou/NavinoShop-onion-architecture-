
using Shared.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comments.Application.Contract.CommentService.Command
{
    public interface ICommentCommandService
    {
       Task< OperationResult> Create(CreateCommentCommandModel command);
        Task<OperationResult> Reject(RejectCommentCommandModel command);
        Task<OperationResult> AcceptedComment(long id);
    }
}
