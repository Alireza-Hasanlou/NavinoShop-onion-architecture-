using Shared.Application;
using Shared.Domain.Enums;


namespace Financial.Application.Contract.WalletService.Commands
{
    public interface IWalletCommands
    {

        Task<OperationResult> WithdrawAsync(int userId, decimal amount,int ownerId, TransactionFor transactionFor, TransactionSource transactionSource,
             TransactionType transactionType, TransactionPortal potral, string description, string authority);
        Task<OperationResult> DepositAsync(int userId, decimal amount,int ownerid, TransactionFor transactionFor, TransactionSource transactionSource,
             TransactionType transactionType,TransactionPortal potral, string description, string authority);
    }
}
