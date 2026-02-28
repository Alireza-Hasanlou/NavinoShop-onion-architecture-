using Shared.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Application.Contract.WalletService.Commands
{
    public interface IWalletCommands
    {
        Task<OperationResult> WithdrawAsync(int userId, decimal amount);
        Task<OperationResult> DepositAsync(int userId, decimal amount);
    }
}
