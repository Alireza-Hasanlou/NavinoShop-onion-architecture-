using Shared.Domain;

namespace Users.Domain.WalletAgg
{
    public class Wallet : BaseEntityCreate<int>
    {

        public int UserId { get; private set; }
        public decimal Balance { get; private set; }

        public User.Agg.User User { get; private set; }

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