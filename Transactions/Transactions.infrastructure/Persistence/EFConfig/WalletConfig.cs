using Financial.Domain.WalletAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financial.infrastructure.Persistence.EFConfig
{
    internal class WalletConfig : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasMany(t => t.Transactions).WithOne(w=>w.Wallet);
        }
    }
}
