using Shared;
using Shared.Domain.Enums;

namespace Query.Contract.Admin.Financial.Transaction
{
    public class TransactionsPagingModel : BasePaging
    {
        public string Title { get; set; }
        public int pageId { get; set; } = 1;
        public string filter { get; set; } = "";
        public int take { get; set; }
        public TransactionType? transactionType { get; set; }
        public TransactionStatus? transactionStatus { get; set; }
        public TransactionSource? transactionSource { get; set; }
        public TransactionFor? transactionFor { get; set; }
        public List<TransactionListQueryModel> Transactions { get; set; }
    }
}
