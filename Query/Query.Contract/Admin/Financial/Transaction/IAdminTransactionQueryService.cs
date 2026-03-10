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
}
