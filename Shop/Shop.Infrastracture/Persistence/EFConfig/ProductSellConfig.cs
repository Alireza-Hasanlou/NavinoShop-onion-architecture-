using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Domain.ProductSellAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastracture.Persistence.EFConfig
{
    internal class ProductSellConfig : IEntityTypeConfiguration<ProductSell>
    {
        public void Configure(EntityTypeBuilder<ProductSell> builder)
        {
            builder.HasKey(x => x.Id);  

            builder.HasMany(x=>x.OrderItems)
                .WithOne(x=>x.ProductSell)
                .HasForeignKey(x=>x.ProductSellId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(x => x.Seller)
                .WithMany(x => x.ProductSells)
                .HasForeignKey(x=>x.SellerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
