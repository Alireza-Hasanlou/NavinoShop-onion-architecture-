
using Blogs.Domain.BlogAgg;
using Blogs.Domain.BlogCategoryAgg;
using Blogs.Infrastructure.Persistence.EFConfig;
using Microsoft.EntityFrameworkCore;

namespace Blogs.Infrastructure.Persistence.Context
{
    public class NavinoDbContext : DbContext
    {
        public NavinoDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BlogConfig());  
            modelBuilder.ApplyConfiguration(new BlogCategoryConfig());
        }
    }
}
