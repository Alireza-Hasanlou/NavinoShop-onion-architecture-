using Microsoft.EntityFrameworkCore;
using Store.Domain.StoreProductAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Infrastructure.Persistence.Context
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {

        }
        public DbSet<Store.Domain.StoreAgg.Store> Stores { get; set; }
        public DbSet<StoreProduct> StoreProducts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
