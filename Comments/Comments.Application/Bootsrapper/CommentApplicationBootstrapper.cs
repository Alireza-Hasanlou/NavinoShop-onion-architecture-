
using Comments.Application.Contract.CommentService.Command;
using Comments.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
