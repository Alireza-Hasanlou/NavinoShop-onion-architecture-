using Financial.Domain.WalletAgg;
using Shared.Domain;
using Shared.Domain.Enums;

namespace Financial.Domain.TransactionAgg
{
    public class Transaction : BaseEntityCreate<long>
    {
        public Transaction(int userId,
            int price,
            string authority,
            TransactionPortal portal,
            TransactionStatus status,
            TransactionFor transactionFor,
            TransactionType transactionType,
            TransactionSource transactionSource,
            string description,
            int transationById)
        {
            UserId = userId;
            Price = price;
            RefId = "";
            Authority = authority;
            Portal = portal;
            Status = status;
            TransactionFor = transactionFor;
            TransactionType = transactionType;
            TransactionSource = transactionSource;
            Description = description;
            TransationById = transationById;
            Wallet = new Wallet();
        }

        public int UserId { get; private set; }
        public int Price { get; private set; }
        public string? RefId { get; private set; }
        public string Authority { get; private set; }
        public TransactionPortal Portal { get; private set; }
        public TransactionStatus Status { get; private set; }
        public TransactionFor TransactionFor { get; private set; }
        public TransactionType TransactionType { get; private set; }
        public TransactionSource TransactionSource { get; private set; }
        public string Description { get; private set; }
        public int TransationById { get; private set; } // If transaction crate by Admin
        public Wallet Wallet { get; set; }
        public bool Payment(TransactionStatus status, string refId)
        {
            Status = status;

            if (!string.IsNullOrEmpty(refId))
            {
                RefId = refId;
            }
            else
            {
                RefId = null;
            }
            return true;
        }



    }
}
