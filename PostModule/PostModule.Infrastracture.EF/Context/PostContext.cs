using Microsoft.EntityFrameworkCore;
using PostModule.Domain.StateEntity;
using PostModule.Domain.CityEntity;
using PostModule.Domain.PostEntity;
using PostModule.Domain.UserPostAgg;
using PostModule.Domain.SettingAgg;
using PostModule.Infrastracture.EFConfig;

namespace PostModule.Infrastracture.Context
{
    public class PostContext : DbContext
    {
        public PostContext(DbContextOptions<PostContext> options) : base(options)
        {

        }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostPrice> PostPrices { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<PostOrder> PostOrders { get; set; }
        public DbSet<UserPost> UserPosts { get; set; }
        public DbSet<PostSetting> PostSettings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StateMapping).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
