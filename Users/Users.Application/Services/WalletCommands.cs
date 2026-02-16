using Shared.Application;
using Shared.Application.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Contract.WalletService.Commands;
using Users.Domain.WalletAgg;

namespace Users.Application.Services
{
    internal class WalletCommands : IWalletCommands
    {
        private readonly IWalletRepository _walletRepository;

        public WalletCommands(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<OperationResult> Deposit(int userId, decimal amount)
        {
            if (amount < 1000)
                return new OperationResult(false, ValidationMessages.PaymentPriceError);
            var wallet = await _walletRepository.GetWalletByUserIdAsync(userId);
            if (wallet == null)
                return new OperationResult(false, ValidationMessages.SystemErrorMessage);
            wallet.Deposit(amount);
            var res = await _walletRepository.SaveAsync();
            if (res)
                return new OperationResult(true);
            return new OperationResult(false, ValidationMessages.SystemErrorMessage);
        }

        public async Task<OperationResult> Withdraw(int userId, decimal amount)
        {
            if (amount < 1000)
                return new OperationResult(false, ValidationMessages.PaymentPriceError);
            var wallet = await _walletRepository.GetWalletByUserIdAsync(userId);
            if (wallet == null)
                return new OperationResult(false, ValidationMessages.SystemErrorMessage);
            wallet.Withdraw(amount);
            var res = await _walletRepository.SaveAsync();
            if (res)
                return new OperationResult(true);
            return new OperationResult(false, ValidationMessages.SystemErrorMessage);
        }
    }
}
