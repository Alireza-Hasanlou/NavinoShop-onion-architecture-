
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Shop.Domain.OrderAddressAgg;
using Shop.Domain.OrderAgg;
using Shop.Domain.OrderItemAgg;
using Shop.Domain.OrderSellerAgg;
using Shop.Domain.ProductAgg;
using Shop.Domain.ProductCategoryAgg;
using Shop.Domain.ProductFreatureAgg;
using Shop.Domain.ProductGalleryAgg;
using Shop.Domain.ProductSellAgg;
using Shop.Domain.Relations.ProductCategoryRel;
using Shop.Domain.SellerAgg;
using Shop.Domain.ShopSettingAgg;
using Shop.Infrastracture.Persistence.EFConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastracture.Persistence.Context
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {

        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderAddress> OrderAddresses { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderSeller> OrderSellers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductFreature> ProductFreatures { get; set; }
        public DbSet<ProductGallery> ProductGalleries { get; set; }
        public DbSet<ProductSell> productSells { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<ShopSetting> shopSettings { get; set; }
        public DbSet<Product_Category_Rel> product_Category_Rels { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfig).Assembly);

            base.OnModelCreating(modelBuilder);
        }
        
    }
    public class ShopContextFactory
        : IDesignTimeDbContextFactory<ShopContext>
    {
        public ShopContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ShopContext>();

            optionsBuilder.UseSqlServer(
                "Server=.;Database=NavinoShop_DB;Trusted_Connection=True;TrustServerCertificate=True;");

            return new ShopContext(optionsBuilder.Options);
        }
    }

}
