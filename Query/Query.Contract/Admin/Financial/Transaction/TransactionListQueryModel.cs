using Shared.Domain.Enums;

namespace Query.Contract.Admin.Financial.Transaction
{
    public class TransactionListQueryModel
    {
        public int UserId { get; set; }
        public string RefId { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public decimal Price { get; set; }
        public string transationBy_Name { get; set; } // The name of the person who made the transaction.
        public int transationBy_Id { get; set; } //The Id of the person who made the transaction.
        public string Description { get; set; }
        public TransactionType TransactionType { get; set; }
        public TransactionFor transactionFor { get; set; }
        public TransactionStatus transactionStatus { get; set; }
        public TransactionSource TransactionSource { get; set; }
        public string TransactionDate { get; set; }
    }
}
