using Financial.Domain.TransactionAgg;
using Financial.Domain.WalletAgg;
using Financial.infrastructure.Persistence.Context;
using Financial.infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Financial.infrastructure.Bootstrapper
{
    public class Financial_infrastructure_Bootstrapper
    {
        public static void Config(IServiceCollection services, string ConnectionString)
        {
            services.AddDbContext<FinancialContext>(option =>
            {
                option.UseSqlServer(ConnectionString);
            });
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddTransient<IWalletRepository, WalletRepository>();
        }
    }
}
