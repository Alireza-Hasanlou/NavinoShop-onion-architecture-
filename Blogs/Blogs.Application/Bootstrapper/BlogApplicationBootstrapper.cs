using Blogs.Application.Contract.BlogApplication.Command;
using Blogs.Application.Contract.BlogCategoryService.Command;
using Blogs.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Application.Bootstrapper
{
    public class BlogApplicationBootstrapper
    {
        public static void Config(IServiceCollection services)
        {
            services.AddScoped<IBlogCategoryCommandService, BlogCategoryService>();
            services.AddScoped<IBlogCommandService, BlogService>();
        }
    }
}
