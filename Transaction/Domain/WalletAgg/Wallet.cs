using Financial.Domain.TransactionAgg;
using Shared.Domain;

namespace Financial.Domain.WalletAgg
{
    public class Wallet : BaseEntityCreate<int>
    {

        public int UserId { get; private set; }
        public decimal Balance { get; private set; }
        public ICollection<Transaction> Transactions{ get; private set; }

        public Wallet(int userId, decimal balance)
        {
            UserId = userId;
            Balance = balance;
            Transactions = new List<Transaction>();
        }

        public void Deposit(decimal amount)
        {
            Balance += amount;
        }

        public bool Withdraw(decimal amount)
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