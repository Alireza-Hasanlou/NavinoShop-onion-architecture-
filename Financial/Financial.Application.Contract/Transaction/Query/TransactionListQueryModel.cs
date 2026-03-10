using Shared;
using Shared.Domain.Enums;

namespace Financial.Application.Contract.Transaction.Query
{
    public class TransactionListLoading : BasePaging
    {
        public List<TransactionListQueryModel> transactions { get; set; }
    }
    public class TransactionListQueryModel
    {
        public decimal Price { get; set; }
        public TransactionType TransactionType { get; set; }
        public string TransactionSource { get; set; }
        public string TransactionDate { get; set; }
        
    }
}
