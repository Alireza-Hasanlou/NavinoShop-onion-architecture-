using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Query.Contract.Admin.Transaction;
using Shared.Application;
using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.User.Agg.IRepository;

namespace Query.Service.Admin.Transaction
{
    internal class AdminTransactionQueryService : IAdminTransactionQueryService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;

        public AdminTransactionQueryService(ITransactionRepository transactionRepository, IUserRepository userRepository)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
        }

        public async Task<TransactionsPagingModel> GetTransactionsForAdmin(int take, int pageId, string filter
            , TransactionFor transactionFor, TransactionSource transactionSource,
            TransactionStatus transactionStatus, TransactionType transactionType)
        {
            var model = new TransactionsPagingModel();
            var transactions = _transactionRepository.GetAll();
            if (!string.IsNullOrWhiteSpace(filter))
            {
                transactions = transactions.Where(t => t.RefId.Equals(filter)
                || t.Authority.Contains(filter)
                || t.Description.Contains(filter));
            }
            if (transactionType != TransactionType.همه)
            {
                transactions = transactions.Where(t => t.TransactionType == transactionType);
            }
            if (transactionStatus != TransactionStatus.همه)
            {
                transactions = transactions.Where(t => t.Status == transactionStatus);
            }
            if (transactionSource != TransactionSource.همه)
            {
                transactions = transactions.Where(t => t.TransactionSource == transactionSource);
            }
            if (transactionFor != TransactionFor.All)
            {
                transactions = transactions.Where(t => t.TransactionFor == transactionFor);
            }
            model.GetData(transactions, pageId, take, 5);
            model.filter = filter;
            model.transactionFor = transactionFor;
            model.transactionType = transactionType;
            model.transactionSource = transactionSource;
            model.transactionStatus = transactionStatus;
            model.Transactions = await transactions.OrderByDescending(d => d.CreateDate)
                .Skip(model.Skip)
                .Take(model.Take)
                .Select(t => new TransactionListQueryModel
                {
                    UserId = t.UserId,
                    RefId = t.RefId,
                    UserName = "",
                    Mobile = "",
                    Price = t.Price,
                    TransactionDate = t.CreateDate.ToPersainDate(),
                    TransactionSource = t.TransactionSource,
                    TransactionType = t.TransactionType,
                    transactionFor = t.TransactionFor,
                    transactionStatus = t.Status
                })
                .ToListAsync();
            var userIds = await transactions.Select(u => u.UserId).ToListAsync();
            var users = await _userRepository.GetUsersByIds(userIds);

            foreach (var item in model.Transactions)
            {
                var user = users.Single(i => i.Id.Equals(item.UserId));
                item.UserName = user.FullName;
                item.Mobile = user.Mobile;
            }

            return model;
        }
    }
}
