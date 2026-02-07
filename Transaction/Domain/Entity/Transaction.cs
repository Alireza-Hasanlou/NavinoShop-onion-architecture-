using Shared.Domain;
using Shared.Domain.Enums;

namespace Domain.Entity
{
    public class Transaction:BaseEntityCreate<long>
    {
        public Transaction(int userId, int price, string authority,
             TransactionFor transactionFor, int ownerId)
        {
            UserId = userId;
            Price = price;
            RefId = "";
            Authority = authority;
            Portal = TransactionPortal.زرین_پال;
            Status = TransactionStatus.نا_موفق;
            TransactionFor = transactionFor;
            OwnerId = ownerId;
        }

        public void Payment(TransactionStatus status, string refid)
        {
            Status = TransactionStatus.موفق;
            RefId = refid;
        }
        public int UserId { get; private set; }
        public int Price { get; private set; }
        public string RefId { get; private set; }
        public string Authority { get; private set; }
        public TransactionPortal Portal { get; private set; }
        public TransactionStatus Status { get; private set; }
        public TransactionFor TransactionFor { get; private set; }
        public int OwnerId { get; private set; }

    }
}
