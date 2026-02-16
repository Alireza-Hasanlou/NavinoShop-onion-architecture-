using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Query.Contract.Admin.User;
using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Contract.UserService.Query;
using Users.Domain.User.Agg.IRepository;
using Users.Domain.WalletAgg;

namespace Query.Service.Admin.User
{
    internal class AdminUserQueryService : IAdminUserQueryService
    {
        private readonly IUserRepository _userRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;

        public AdminUserQueryService(IUserRepository userRepository, IWalletRepository walletRepository, ITransactionRepository transactionRepository)
        {
            _userRepository = userRepository;
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<UserDetailQueryModel> GetUserDetailAsync(int userId)
        {
            var user = await _userRepository.GetUserDetailAsync(userId);
            var wallet = await _walletRepository.GetWalletByUserIdAsync(userId);
            var transactions = await _transactionRepository.GetAllBy(u => u.UserId == userId
            && u.TransactionFor == TransactionFor.Wallet
            && u.Status == TransactionStatus.موفق).ToListAsync();
            return new UserDetailQueryModel
            {
                UserId = user.Id,
                FullName = user.FullName,
                Avatar = user.Avatar,
                Active = user.Active,
                Email = user.Email,
                Mobile = user.Mobile,
                CreateDate = user.CreateDate,
                IsDelete = user.IsDelete,
                WalletBalancr = wallet.Balance,
                SuccessTransactionCount = transactions.Count(),
                successTransactionSum = transactions.Sum(p => p.Price),
            };
        }
    }
}
