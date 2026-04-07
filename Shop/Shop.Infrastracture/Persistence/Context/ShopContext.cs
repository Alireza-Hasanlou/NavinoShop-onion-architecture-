
using Microsoft.EntityFrameworkCore;
using Shop.Domain.OrderAddressAgg;
using Shop.Domain.OrderAgg;
using Shop.Domain.OrderItemAgg;
using Shop.Domain.OrderSellerAgg;
using Shop.Domain.Product_SellerAgg;
using Shop.Domain.ProductAgg;
using Shop.Domain.ProductCategoryAgg;
using Shop.Domain.ProductFreatureAgg;
using Shop.Domain.ProductGalleryAgg;
using Shop.Domain.Relations.ProductCategoryRel;
using Shop.Domain.SellerAgg;
using Shop.Domain.ShopSettingAgg;
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
        public DbSet<ProductFreature> GetProductFreatures { get; set; }
        public DbSet<ProductGallery> ProductGalleries { get; set; }
        public DbSet<Product_Seller> Product_Sellers { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<ShopSetting> shopSettings { get; set; }
        public DbSet<Product_Category_Rel> product_Category_Rels { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}
