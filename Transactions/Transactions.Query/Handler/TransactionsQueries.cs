using Domain.Entity;
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

        public async Task<TransactionDetailDto> GetTransactionDetail(long id)
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
    }
}
