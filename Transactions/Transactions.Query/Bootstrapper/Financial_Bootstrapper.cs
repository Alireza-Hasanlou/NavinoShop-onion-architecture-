using Financial.Application.Contract.Transaction.Query;
using Financial.Application.Contract.WalletService.Query;
using Financial.infrastructure.Bootstrapper;
using Financial.Query.Handler;
using Microsoft.Extensions.DependencyInjection;

namespace Financial.Query.Bootstrapper
{
    public class Financial_Bootstrapper
    {
        public static void Config(IServiceCollection services, string ConnectionString)
        {
            Financial_infrastructure_Bootstrapper.Config(services, ConnectionString);

            services.AddTransient<ITransactionQueries, TransactionsQueries>();
            services.AddTransient<IWalletQueries, WalletQueries>();
        }
    }
}
