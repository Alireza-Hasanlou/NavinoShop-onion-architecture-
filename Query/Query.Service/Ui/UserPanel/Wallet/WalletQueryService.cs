using Financial.Domain.TransactionAgg;
using Financial.Domain.WalletAgg;
using Microsoft.EntityFrameworkCore;
using Query.Contract.UI.UserPanel.Wallet;
using Shared.Application;
using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
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
            if(wallet==null)
                return new();

            var transactions = await _transactionRepository.GetAllBy(t => t.UserId == userId
            && t.TransactionFor == TransactionFor.Wallet
            && t.Status == TransactionStatus.موفق)
             .ToListAsync();
            decimal totalDeposits = transactions.Where(t => t.TransactionType == TransactionType.واریز).Sum(p => p.Price);
            decimal totalWithdraw = transactions.Where(t => t.TransactionType == TransactionType.برداشت).Sum(p => p.Price);

            var lastTransactions = transactions.Take(5).Select(t => new TransactionQueryModel
            {
                Price = t.Price,
                TransactionType = t.TransactionType,
                TransactionSource = t.TransactionSource.ToString().Replace("_", ""),
                TransactionDate = t.CreateDate.ToPersainDate()
            }).ToList();

            return new WalletDetailQueryModel
            {
                Balance = wallet.Balance,
                TotalDeposits = totalDeposits,
                TotalWithdrawals = totalWithdraw,
                LastTransactions = lastTransactions
            };

        }
    }
}
