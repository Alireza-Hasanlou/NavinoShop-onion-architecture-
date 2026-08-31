using Financial.Domain.TransactionAgg;
using Shared.Domain;

namespace Financial.Domain.WalletAgg
{
    public class Wallet : BaseEntityCreate<int>
    {

        public int OwnerId { get; private set; }
        public int Balance { get; private set; }
        public ICollection<Transaction> Transactions { get; private set; }


        public static Wallet Create(int userId)
        {
            return new Wallet()
            {
                OwnerId = userId,
                Balance = 0,
                Transactions = new List<Transaction>()
            };
        }
        public void Deposit(int amount)
        {
            if (amount >= 100)
            {
                Balance += amount;
            }

        }

        public bool Withdraw(int amount)
        {
            if (Balance >= amount)
            {
                Balance -= amount;
                return true;
            }
            return false;
        }
    }
}