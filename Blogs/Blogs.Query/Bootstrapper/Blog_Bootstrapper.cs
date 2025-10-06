using Blogs.Application.Contract.BlogApplication.Query;
using Blogs.Application.Contract.BlogCategoryService.Query;
using Blogs.Infrastructure.Bootstrapper;
using Blogs.Query.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Query.Bootstrapper
{
    public static class Blog_Bootstrapper
    {
        public static void Config(IServiceCollection services, string connectionString)
        {
            BlogInfrastructureBootstrapper.Config(services, connectionString);
            services.AddScoped<IBlogCategoryQueryService, BlogCategoryQuery>();
            services.AddScoped<IBlogQueryService, BlogQuery>();
        }

    }
}
