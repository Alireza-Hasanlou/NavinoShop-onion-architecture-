using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Transactions.infrastructure.Persistence.Context;
using Transactions.infrastructure.Persistence.Repository;


namespace Transactions.infrastructure.Bootstrapper
{
    public class Transaction_infrastructure_Bootstrapper
    {
        public static void Config(IServiceCollection services, string ConnectionString)
        {
            services.AddDbContext<TransactionContext>(option =>
            {
                option.UseSqlServer(ConnectionString);
            });
            services.AddTransient<ITransactionRepository, TransactionRepository>();
        }
    }
}
