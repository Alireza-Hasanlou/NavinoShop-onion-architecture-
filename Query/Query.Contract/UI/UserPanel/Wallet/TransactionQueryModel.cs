using Shared.Domain.Enums;

namespace Query.Contract.UI.UserPanel.Wallet
{
    public class TransactionQueryModel
    {
        public decimal Price { get; set; }
        public TransactionType  TransactionType { get; set; }
        public string TransactionSource { get; set; }
        public string TransactionDate { get; set; }
    }
}
