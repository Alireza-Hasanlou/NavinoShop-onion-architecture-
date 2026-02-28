using Shared.Application;
using Shared.Domain.Enums;
using System.ComponentModel;
namespace Financial.Application.Contract.Transaction.Command
{
    public interface ITransactionCommands
    {
        Task<OperationResult> CreateAsync(CreateTransacionCommandModel commnad);
        Task<OperationResult> Payment(long id,string refid); 
    }

}
