using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Transactions.infrastructure.Persistence.EFConfig
{
    internal class TransactionConfig : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.RefId).IsRequired(false).HasMaxLength(100);
            builder.Property(x => x.TransactionFor).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.Portal).IsRequired();
        }
    }
}
