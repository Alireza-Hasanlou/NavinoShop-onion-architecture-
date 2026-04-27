using Shared.Application;
using Shared.Domain.Enums;
namespace Financial.Application.Contract.Transaction.Command
{
    public interface ITransactionCommands
    {
        Task<OperationResult> CreateAsync(CreateTransacionCommandModel commnad);
        Task<OperationResult> DeleteAsync(long transationId);
        Task<OperationResult> Payment(TransactionStatus status, long id,string refid); 
    }

}
