using Microsoft.Extensions.DependencyInjection;
using PostModule.Application.Bootsrapper;
using PostModule.Application.Contract.PostQuery;
using PostModule.Application.Contract.StateQuery;
using PostModule.Application.Contract.UserPostApplication.Query;
using PostModule.Application.Services;
using PostModule.Domain.Services;
using PostModule.Infrastracture.Bootstrapper;
using PostModule.Query.Services;

namespace PostModule.Query.Bootstrapper
{
    public static class  Post_Bootstrapper
    {
        public static void Config(IServiceCollection services , string connectionString)
        {
            PostInfrastructureBootstrapper.Config(services, connectionString);
            PostApplicationBootstrapper.Config(services);

            services.AddTransient<IPackageQueryService, PackageQueryService>();
            services.AddTransient<IPostQuery, PostQuery>();
            services.AddTransient<IPackageQueryService, PackageQueryService>();
        }
    }
}