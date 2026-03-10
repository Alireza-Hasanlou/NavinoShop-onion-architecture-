using Financial.Application.Contract.Transaction.Command;
using Financial.Application.Contract.WalletService.Commands;
using Financial.Application.Handlers.TransactionHandler;
using Financial.Application.Handlers.WalletHandler;
using Microsoft.Extensions.DependencyInjection;



namespace Financial.Application.Bootstrapper
{
    public class Financial_Application_Bootstrapper
    {
        public static void Config(IServiceCollection services)
        {
            services.AddTransient<ITransactionCommands, TransactionCommands>();
            services.AddTransient<IWalletCommands,WalletCommands>();
        }
    }
}
