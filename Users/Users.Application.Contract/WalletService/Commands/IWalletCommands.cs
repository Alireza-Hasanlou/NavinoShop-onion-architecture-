using Shared.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Application.Contract.WalletService.Commands
{
    public interface IWalletCommands
    {
        Task<OperationResult> Withdraw(int userId, decimal amount);
        Task<OperationResult> Deposit(int userId, decimal amount);
    }
}
