using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Shared.Application;
using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Application.Contract.Transaction.Command;
using Transactions.Application.Contract.Transaction.Query;

namespace Transactions.Query.Handler
{
    internal class TransactionsQueries : ITransactionQueries
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionsQueries(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<TransactionDetailDto> GetTransactionDetailAsync(long id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            return new TransactionDetailDto
            {
                UserId = transaction.UserId,
                OwnerId = transaction.OwnerId,
                Authority = transaction.Authority,
                Portal = transaction.Portal,
                Price = transaction.Price,
                RefId = transaction.RefId,
                Status = transaction.Status,
                TransactionFor = transaction.TransactionFor,
            };
        }

        public async Task<List<TransactionListQueryModel>> GetTransactionsForUserAsync(int UserId, TransactionFor transactionFor)
        {
            return await _transactionRepository.GetAllBy(t => t.UserId == UserId
            && t.TransactionFor == transactionFor
            && t.Status == TransactionStatus.موفق)
                .Skip(5)
                .Take(5)
                .Select(t => new TransactionListQueryModel
                {
                    Price = t.Price,
                    TransactionType = t.TransactionType,
                    TransactionSource = t.TransactionSource.ToString().Replace("_", ""),
                    TransactionDate = t.CreateDate.ToPersainDate()
                }).ToListAsync();


        }
    }
}
