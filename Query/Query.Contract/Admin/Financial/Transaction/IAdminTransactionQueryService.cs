using Shared;
using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Contract.Admin.Financial.Transaction
{
    public interface IAdminTransactionQueryService
    {
        Task<TransactionsPagingModel> GetTransactionsForAdmin(int take , int pageId, string filter ,int userId
            , TransactionFor transactionFor, TransactionSource transactionSource,
            TransactionStatus transactionStatus, TransactionType transactionType);
    }
  
    public class TransactionsPagingModel : BasePaging
    {
        public string Title { get; set; }
        public int pageId { get; set; } = 1;
        public string filter { get; set; } = "";
        public int take { get; set; }
        public TransactionType? transactionType { get; set; }
        public TransactionStatus? transactionStatus { get; set; }
        public TransactionSource? transactionSource { get; set; }
        public TransactionFor? transactionFor { get; set; }
        public List<TransactionListQueryModel> Transactions { get; set; }
    }
    public class TransactionListQueryModel
    {
        public int UserId { get; set; }
        public string RefId { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public decimal Price { get; set; }
        public TransactionType TransactionType { get; set; }
        public TransactionFor transactionFor { get; set; }
        public TransactionStatus transactionStatus { get; set; }
        public TransactionSource TransactionSource { get; set; }
        public string TransactionDate { get; set; }
    }
}
