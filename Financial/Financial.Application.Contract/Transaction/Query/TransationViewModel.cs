using Shared.Domain.Enums;

namespace Financial.Application.Contract.Transaction.Query
{
    public class TransationViewModel
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public int Price { get; set; }
        public string? RefId { get; set; }
        public TransactionPortal Portal { get; set; }
        public TransactionStatus Status { get; set; }
        public TransactionFor TransactionFor { get; set; }

    }
}
