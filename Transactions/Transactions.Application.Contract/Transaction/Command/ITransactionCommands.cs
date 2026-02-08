using Shared.Application;
using Shared.Domain.Enums;
using System.ComponentModel;
namespace Transactions.Application.Contract.Transaction.Command
{
    public interface ITransactionCommands
    {
        Task<OperationResult> CreateAsync(CreateTransacionCommandModel commnad);
        Task<OperationResult> Payment(TransactionStatus status,long id,string refid); 
    }

}
