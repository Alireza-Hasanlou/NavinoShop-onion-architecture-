using Financial.Application.Contract.Transaction.Query;
using Financial.Domain.TransactionAgg;
using Shared.Application;
using Shared.Domain.Enums;

namespace Financial.Query.Handler
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

        public async Task<TransactionListLoading> GetTransactionsForUserAsync(int pageId, int UserId, TransactionFor transactionFor)
        {
            pageId++;
            var model = new TransactionListLoading();
            var transations = _transactionRepository.GetAllBy(t => t.OwnerId == UserId
            && t.TransactionFor == transactionFor
            && t.Status == TransactionStatus.موفق);
            model.GetData(transations, pageId, 3, 5);
            model.transactions = transations
                .OrderByDescending(d=>d.CreateDate)
                .Skip(model.Skip)
                .Take(model.Take)
                .Select(t => new TransactionListQueryModel
                {
                    Price = t.Price,
                    TransactionType = t.TransactionType,
                    TransactionSource = t.TransactionSource.ToString().Replace("_", " "),
                    TransactionDate = t.CreateDate.ToPersainDate()
                }).ToList();
            return model;
        }
    }
}
