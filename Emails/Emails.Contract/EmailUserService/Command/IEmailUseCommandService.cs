using Utility.Shared.Application;

namespace Emails.Application.Contract.EmailUserService.Command
{
    public interface IEmailUseCommandService
    {
        Task<OperationResult> Create(CreateEmailUserCommandModel command);
        Task<OperationResult> ActivationChange(int id);
    }
}
