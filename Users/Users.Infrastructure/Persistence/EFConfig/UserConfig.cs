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
    internal class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);

            builder.Property(b => b.FullName).IsRequired(false).HasMaxLength(255);
            builder.Property(b => b.Avatar).IsRequired().HasMaxLength(200);
            builder.Property(b => b.Email).IsRequired(false).HasMaxLength(255);
            builder.Property(b => b.Mobile).IsRequired().HasMaxLength(11);
            builder.Property(b => b.UserGender).IsRequired();
            builder.Property(b => b.Password).IsRequired().HasMaxLength(100);

            builder.HasMany(a => a.Addresses)
                .WithOne(a => a.User)
                .HasForeignKey(k => k.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.UserRoles)
                .WithOne(r => r.User)
                .HasForeignKey(k => k.UserId)
                .OnDelete(DeleteBehavior.Cascade);

       
        }
    }
}
