
using Comments.Application.Contract.CommentService.Command;
using Comments.Application.Services;
using Microsoft.Extensions.DependencyInjection;
namespace Comments.Application.Bootsrapper
{
    public static class CommentApplicationBootstrapper
    {
        public static void Config(IServiceCollection services)
        {
            services.AddTransient<ICommentCommandService, CommentService>();
        }
    }
}
