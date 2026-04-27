using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Application.Contract.Transaction.Query
{
    public interface ITransactionQueries
    {
        Task<TransactionDetailDto> GetTransactionDetailAsync(long id);
        Task<TransactionListLoading> GetTransactionsForUserAsync(int pageId,int UserId, TransactionFor transactionFor);
        Task<TransationViewModel> GetTransationForPayment(long transationId);
    }
}
