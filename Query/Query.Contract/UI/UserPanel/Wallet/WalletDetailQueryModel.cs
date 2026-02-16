namespace Query.Contract.UI.UserPanel.Wallet
{
    public class WalletDetailQueryModel
    {
        public decimal Balance { get; set; }
        public decimal TotalDeposits { get; set; }
        public decimal TotalWithdrawals { get; set; }
        public List<TransactionQueryModel> LastTransactions { get; set; }

    }
}
