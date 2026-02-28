using Financial.Domain.WalletAgg;
using Shared.Domain;
using Shared.Domain.Enums;

namespace Financial.Domain.TransactionAgg
{
    public class Transaction : BaseEntityCreate<long>
    {
        public Transaction(int userId,
            decimal price,
            string authority,
            TransactionPortal portal,
            TransactionStatus status,
            TransactionFor transactionFor,
            TransactionType transactionType,
            TransactionSource transactionSource,
            string description,
            int ownerId)
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
            OwnerId = ownerId;
        }

        public int UserId { get; private set; }
        public decimal Price { get; private set; }
        public string? RefId { get; private set; }
        public string Authority { get; private set; }
        public TransactionPortal Portal { get; private set; }
        public TransactionStatus Status { get; private set; }
        public TransactionFor TransactionFor { get; private set; }
        public TransactionType TransactionType { get; private set; }
        public TransactionSource TransactionSource { get; private set; }
        public string Description { get; private set; }
        public int OwnerId { get; private set; }
        public Wallet Wallet { get; set; }
        public bool Payment(string refId)
        {
            if (Status == TransactionStatus.موفق)
                return false;

            Status = TransactionStatus.موفق;

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
