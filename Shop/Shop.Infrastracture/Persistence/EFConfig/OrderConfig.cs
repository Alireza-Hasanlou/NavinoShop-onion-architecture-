using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Domain.OrderAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastracture.Persistence.EFConfig
{
    internal class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);  
            builder.HasMany(x=>x.OrderSellers)
                .WithOne(x=>x.Order)
                .HasForeignKey(x=>x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.OrderAddress)
                .WithOne(x => x.Order)
                .HasForeignKey<Order>(x => x.OrderAddressId);
             


        }
    }
}
