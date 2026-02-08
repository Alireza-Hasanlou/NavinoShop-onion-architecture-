using Domain.Entity;
using Shared.Insfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.infrastructure.Persistence.Context;

namespace Transactions.infrastructure.Persistence.Repository
{
    internal class TransactionRepository : GenericRepository<Transaction, long>, ITransactionRepository
    {
        private readonly TransactionContext _Context;

        public TransactionRepository(TransactionContext context) : base(context)
        {
            _Context = context;
        }
    }
}
