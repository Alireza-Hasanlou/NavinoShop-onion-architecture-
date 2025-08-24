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
    internal class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Title).IsRequired().HasMaxLength(255);
            builder.HasMany(r=>r.UserRoles).WithOne(r => r.Role).HasForeignKey(k=>k.RoleId);
            builder.HasMany(p=>p.RolePermissions).WithOne(p=>p.Role).HasForeignKey(p=>p.RoleId);

        }
    }
}
