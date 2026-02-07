using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Application.Contract.Transaction.Command;
using Transactions.Application.Handlers.TransactionHandler;

namespace Transactions.Application.Bootstrapper
{
    public class Transaction_Application_Bootstrapper
    {
        public static void Config(IServiceCollection services)
        {
            services.AddTransient<ITransactionCommands, TransactionCommands>();
        }
    }
}
