using Comments.Domain.CommentAgg;
using Comments.Infrastructure.Persistence.EFConfig;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Comments.Infrastructure.Persistence.Context
{
    public class CommentContext : DbContext
    {
        public CommentContext(DbContextOptions<CommentContext> options) : base(options) 
        {
            
        }
        public DbSet<Comment> Comments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = typeof(CommentConfig).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly); 
            base.OnModelCreating(modelBuilder);
        }
    }
}