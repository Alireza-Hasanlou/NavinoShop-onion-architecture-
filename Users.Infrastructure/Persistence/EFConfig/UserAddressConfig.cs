using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.User.Agg;

namespace Users.Infrastructure.Persistence.EFConfig
{
    internal class UserAddressConfig : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.ToTable("UserAddresses");
            builder.HasKey(b => b.Id);

            builder.Property(b => b.FullName).IsRequired().HasMaxLength(255);
            builder.Property(b => b.AddressDetail).IsRequired().HasMaxLength(500);
            builder.Property(b => b.NationalCode).IsRequired(false).HasMaxLength(10);
            builder.Property(b => b.Phone).IsRequired().HasMaxLength(11);
            builder.Property(b => b.PostalCode).IsRequired().HasMaxLength(10);

            builder.HasOne(a => a.User)
                .WithMany(a => a.Addresses)
                .HasForeignKey(k => k.UserId);
        }
    }
}
