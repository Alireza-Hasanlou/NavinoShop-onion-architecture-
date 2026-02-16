namespace Query.Contract.Admin.User
{
    public class UserDetailQueryModel
    {
        public int UserId { get; set; }
        public string Avatar { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Active { get; set; }
        public bool IsDelete { get; set; }
        public decimal WalletBalancr { get; set; }
        public int SuccessTransactionCount { get; set; }
        public decimal successTransactionSum { get; set; }

    }
}
