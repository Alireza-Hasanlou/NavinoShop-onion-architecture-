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
    internal class PermissionConfig : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions");
            builder.HasKey(b => b.Id);

            builder.HasMany(r=>r.RolePermissions).WithOne(r=>r.Permission).HasForeignKey(k=>k.PermissionId);

            
        }
    }
}
