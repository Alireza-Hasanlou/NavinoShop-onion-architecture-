using Financial.Domain.TransactionAgg;
using Financial.infrastructure.Persistence.Context;
using Shared.Insfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.infrastructure.Persistence.Repository
{
    internal class TransactionRepository : GenericRepository<Transaction, long>, ITransactionRepository
    {
        private readonly FinancialContext _Context;

        public TransactionRepository(FinancialContext context) : base(context)
        {
            _Context = context;
        }
    }
}
