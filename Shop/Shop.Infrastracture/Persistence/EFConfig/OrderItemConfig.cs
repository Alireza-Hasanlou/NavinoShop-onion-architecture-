using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Domain.OrderItemAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastracture.Persistence.EFConfig
{
    internal class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(x => x.Id);  
            builder.HasOne(x=>x.OrderSeller)
                .WithMany()
                .HasForeignKey(x=>x.OrderSellerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x=>x.ProductSell)
                .WithMany(x=>x.OrderItems)
                .HasForeignKey(x=>x.ProductSellId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
