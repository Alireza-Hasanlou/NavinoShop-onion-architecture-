using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Domain.SellerAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastracture.Persistence.EFConfig
{
    internal class SellerConfig : IEntityTypeConfiguration<Seller>
    {
        public void Configure(EntityTypeBuilder<Seller> builder)
        {
           builder.HasKey(x => x.Id);

            builder.HasMany(x => x.OrderSellers)
                .WithOne(x => x.Seller)
                .HasForeignKey(x => x.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x=>x.ProductSells)
                .WithOne(x=>x.Seller)
                .HasForeignKey(x=>x.SellerId)
                .OnDelete(DeleteBehavior.Restrict);
                
        }
    }
}
