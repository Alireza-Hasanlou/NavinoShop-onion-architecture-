using Financial.Domain.TransactionAgg;
using Financial.Domain.WalletAgg;
using Financial.infrastructure.Persistence.Context;
using Financial.infrastructure.Persistence.EFConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Financial.infrastructure.Persistence.Context
{
    public class FinancialContext : DbContext
    {
        public FinancialContext(DbContextOptions<FinancialContext> options) : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly( typeof(WalletConfig).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
public class FinancialContextFactory
        : IDesignTimeDbContextFactory<FinancialContext>
{
    public FinancialContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FinancialContext>();

        optionsBuilder.UseSqlServer(
            "Server=.;Database=NavinoShop_DB;Trusted_Connection=True;TrustServerCertificate=True;");

        return new FinancialContext(optionsBuilder.Options);
    }
}
