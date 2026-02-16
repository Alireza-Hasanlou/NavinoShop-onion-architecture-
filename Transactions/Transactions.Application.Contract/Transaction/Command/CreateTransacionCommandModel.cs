using Shared.Domain.Enums;
using System.ComponentModel;
namespace Transactions.Application.Contract.Transaction.Command
{
    public class CreateTransacionCommandModel
    {
        public int UserId { get; set; }
        public int OwnerId { get; set; }
        public string Authority { get; set; }
        [DisplayName("توضیحات")]
        public string? Description { get; set; }
        [DisplayName("مبلِغ")]
        public int Price { get; set; }
        [DisplayName("درگاه")]
        public TransactionPortal Portal { get; set; }
        public TransactionFor TransactionFor { get; set; }
        public TransactionType TransactionType { get; set; }
        public TransactionSource TransactionSource { get; set; }


    }
}
