using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transactions.Application.Contract.Transaction.Query
{
    public interface ITransactionQueries
    {
        Task<TransactionDetailDto> GetTransactionDetail(long id);
    }
}
