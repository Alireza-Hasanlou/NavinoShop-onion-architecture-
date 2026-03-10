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

        public async Task<OperationResult> DepositAsync(int userId, decimal amount, int ownerId, TransactionFor transactionFor, TransactionSource transactionSource,
             TransactionType transactionType, TransactionPortal potral, string description, string authority)
        {
            if (amount < 1000)
                return new OperationResult(false, ValidationMessages.PaymentPriceError);
            var wallet = await _walletRepository.GetWalletByUserIdAsync(ownerId);
            if (wallet == null)
                return new OperationResult(false, ValidationMessages.SystemErrorMessage);
            var newTransaction = await _transactionRepository.CreateAsync(new Transaction(userId, amount, authority, potral, TransactionStatus.نا_موفق,
                transactionFor, transactionType, transactionSource, description, ownerId));
            if (newTransaction.Success)
            {
                wallet.Deposit(amount);
                var Depositres = await _walletRepository.SaveAsync();
                if (Depositres)
                {
                    var transaction = await _transactionRepository.GetFirstByUserIdAsync(userId);
                    transaction.Payment("");
                    var Payres= await _walletRepository.SaveAsync();
                    if(Payres)
                        return new OperationResult(true);

                }

            } 
            return new OperationResult(false, ValidationMessages.SystemErrorMessage);
        }

        public async Task<OperationResult> WithdrawAsync(int userId, decimal amount, int ownerId, TransactionFor transactionFor, TransactionSource transactionSource,
             TransactionType transactionType, TransactionPortal potral, string description, string authority)
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
