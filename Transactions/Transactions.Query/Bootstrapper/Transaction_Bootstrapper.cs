using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Application.Contract.Transaction.Query;
using Transactions.infrastructure.Bootstrapper;
using Transactions.Query.Handler;

namespace Transactions.Query.Bootstrapper
{
    public class Transaction_Bootstrapper
    {
        public static void Config(IServiceCollection services, string ConnectionString)
        {
            Transaction_infrastructure_Bootstrapper.Config(services, ConnectionString);

            services.AddTransient<ITransactionQueries, TransactionsQueries>();
        }
    }
}
