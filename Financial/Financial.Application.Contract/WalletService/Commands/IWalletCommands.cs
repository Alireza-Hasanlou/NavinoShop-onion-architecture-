using Shared.Application;
using Shared.Domain.Enums;


namespace Financial.Application.Contract.WalletService.Commands
{
    public interface IWalletCommands
    {

        Task<OperationResult> WithdrawAsync(int userId, int amount, long transationId);
        Task<OperationResult> DepositAsync(int userId, int amount,long transationId);
    }
}
 