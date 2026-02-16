using Shared.Domain.Enums;

namespace Transactions.Application.Contract.Transaction.Query
{
    public class TransactionListQueryModel
    {
        public decimal Price { get; set; }
        public TransactionType TransactionType { get; set; }
        public string TransactionSource { get; set; }
        public string TransactionDate { get; set; }
    }
}
