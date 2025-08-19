using Blogs.Application.Bootstrapper;
using Blogs.Domain.BlogAgg;
using Blogs.Domain.BlogCategoryAgg;
using Blogs.Infrastructure.Persistence.Context;
using Blogs.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Infrastructure.Bootstrapper
{
    public class BlogInfrastructureBootstrapper
    {
        public static void Config(IServiceCollection services, string connectionString)
        {
            BlogApplicationBootstrapper.Config(services);
            services.AddScoped<IBlogCategoryRepository, BlogCategoryRepository>();
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddDbContext<NavinoDbContext>(option =>
            {
                option.UseSqlServer(connectionString);
            });
        }
    }
}
