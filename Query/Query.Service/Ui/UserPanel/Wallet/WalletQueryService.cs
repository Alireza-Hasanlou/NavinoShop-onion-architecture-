using Financial.Domain.TransactionAgg;
using Financial.Domain.WalletAgg;
using Microsoft.EntityFrameworkCore;
using Query.Contract.UI.UserPanel.Wallet;
using Shared.Application;
using Shared.Domain.Enums;

namespace Query.Service.Ui.UserPanel.Wallet
{
    internal class WalletQueryService : IWalletQueryService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;

        public WalletQueryService(IWalletRepository walletRepository, ITransactionRepository transactionRepository)
        {
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<WalletDetailQueryModel> GetWalletForUserPanel(int userId)
        {
            var wallet = await _walletRepository.GetWalletByUserIdAsync(userId);
            if (wallet == null)
            {

                var res = await _walletRepository.CreateAsync(Financial.Domain.WalletAgg.Wallet.Create(userId));
                if (!res.Success)
                    return new();
                 wallet = await _walletRepository.GetWalletByUserIdAsync(userId);
            }
            var transactions = await _transactionRepository.GetAllBy(t => t.UserId == userId
            && t.TransactionFor == TransactionFor.Wallet
            && t.Status == TransactionStatus.موفق)
             .ToListAsync();
            decimal totalDeposits = transactions.Where(t => t.TransactionType == TransactionType.واریز).Sum(p => p.Price);
            decimal totalWithdraw = transactions.Where(t => t.TransactionType == TransactionType.برداشت).Sum(p => p.Price);

            return new WalletDetailQueryModel
            {
                Balance = wallet.Balance,
                TotalDeposits = totalDeposits,
                TotalWithdrawals = totalWithdraw,
            };

        }
    }
}
