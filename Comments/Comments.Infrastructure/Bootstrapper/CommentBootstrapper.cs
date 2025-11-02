using Comments.Application.Bootsrapper;
using Comments.Domain.CommentAgg;
using Comments.Infrastructure.Persistence.Context;
using Comments.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Comments.Infrastructure.Bootstrapper
{
    public static class CommentBootstrapper
    {
        public static void Config(IServiceCollection services,string connection)
        {
            services.AddTransient<ICommentRepository, CommentRepository>();
            CommentApplicationBootstrapper.Config(services);
            services.AddDbContext<CommentContext>(x =>
            {
                x.UseSqlServer(connection);
            });
        }
    }
}