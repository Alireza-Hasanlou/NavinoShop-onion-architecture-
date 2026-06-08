using Discount.Domain.OrderDiscountAgg;
using Discount.Domain.ProductDiscountAgg;
using Discount.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Infrastructure.Persistence.Context
{
    public class DiscountContext : DbContext
    {
        public DiscountContext(DbContextOptions<DiscountContext> options) : base(options)
        {
        }

        public DbSet<ProductDiscount> ProductDiscounts { get; set; }
        public DbSet<OrderDiscount> OrderDiscounts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

//TODO RemoveThis
public class FinancialContextFactory
        : IDesignTimeDbContextFactory<DiscountContext>
{
    public DiscountContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DiscountContext>();

        optionsBuilder.UseSqlServer(
            "Server=.;Database=NavinoShop_DB;Trusted_Connection=True;TrustServerCertificate=True;");

        return new DiscountContext(optionsBuilder.Options);
    }
}