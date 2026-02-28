using Financial.Application.Contract.WalletService.Commands;
using Financial.Domain.WalletAgg;
using Shared.Application;
using Shared.Application.Validations;


namespace Financial.Application.Handlers.WalletHandler
{
    internal class WalletCommands : IWalletCommands
    {
        private readonly IWalletRepository _walletRepository;

        public WalletCommands(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<OperationResult> DepositAsync(int userId, decimal amount)
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

        public async Task<OperationResult> WithdrawAsync(int userId, decimal amount)
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
