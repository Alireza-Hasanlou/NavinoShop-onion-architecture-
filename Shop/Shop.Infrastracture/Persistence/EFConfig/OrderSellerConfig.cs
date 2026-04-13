using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Domain.OrderSellerAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastracture.Persistence.EFConfig
{
    internal class OrderSellerConfig : IEntityTypeConfiguration<OrderSeller>
    {
        public void Configure(EntityTypeBuilder<OrderSeller> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Seller)
                .WithMany(x => x.OrderSellers)
                .HasForeignKey(x => x.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Order)
                .WithMany(x => x.OrderSellers)
                .HasForeignKey(x=>x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.OrderItems)
                .WithOne(x => x.OrderSeller)
                .HasForeignKey(x=>x.OrderSellerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
