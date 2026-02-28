using Shared.Domain.Enums;

namespace Financial.Application.Contract.Transaction.Query
{
    public class TransactionDetailDto
    {
        public int UserId { get; set; }
        public decimal Price { get; set; }
        public string RefId { get; set; }
        public string Authority { get; set; }
        public TransactionPortal Portal { get; set; }
        public TransactionStatus Status { get; set; }
        public TransactionFor TransactionFor { get; set; }
        public int OwnerId { get; set; }
    }
}
