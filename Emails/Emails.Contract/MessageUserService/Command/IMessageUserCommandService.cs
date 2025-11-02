using Shared.Application;

namespace Emails.Application.Contract.MessageUserService.Command
{
    public interface IMessageUserCommandService
    {
        Task<OperationResult> CreateAsync(CreateMessageUserCommandModel command);
        Task<OperationResult> AnsweredBySMS(int id,string message);
        Task<OperationResult> AnsweredByEmail(int id,string mailMessage);
        Task<OperationResult> AnswerByCall(int id);
    }
}
