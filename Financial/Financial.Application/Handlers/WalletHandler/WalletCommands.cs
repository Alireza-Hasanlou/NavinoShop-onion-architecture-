using Financial.Application.Contract.WalletService.Commands;
using Financial.Domain.TransactionAgg;
using Financial.Domain.WalletAgg;
using Shared.Application;
using Shared.Application.Validations;
using Shared.Domain.Enums;
using System.Net.Http.Headers;


namespace Financial.Application.Handlers.WalletHandler
{
    internal class WalletCommands : IWalletCommands
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;

        public WalletCommands(IWalletRepository walletRepository, ITransactionRepository transactionRepository)
        {
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<OperationResult> DepositAsync(int userId, int amount, long transationId)
        {
            if (amount < 1000)
                return new OperationResult(false, ValidationMessages.PaymentPriceError);
            var wallet = await _walletRepository.GetWalletByUserIdAsync(userId);
            if (wallet == null)
                return new OperationResult(false, ValidationMessages.SystemErrorMessage);

            wallet.Deposit(amount);
            var Depositres = await _walletRepository.SaveAsync();
            if (Depositres)
                return new OperationResult(false);
            return new OperationResult(false, ValidationMessages.SystemErrorMessage);
        }

        public async Task<OperationResult> WithdrawAsync(int userId, int amount, long transationId)
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
