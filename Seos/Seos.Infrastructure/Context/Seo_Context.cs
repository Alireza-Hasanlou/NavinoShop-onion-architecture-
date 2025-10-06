using Microsoft.EntityFrameworkCore;
using Seos.Domain;
using Seos.Domain.SeoAgg;

namespace Seos.Infrastructure.Context
{
    public class Seo_Context : DbContext
    {
        public Seo_Context(DbContextOptions<Seo_Context> options) : base(options)
        {
        }
        public DbSet<Seo> Seos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SeoConfig());

            base.OnModelCreating(modelBuilder);
        }
    }
}
