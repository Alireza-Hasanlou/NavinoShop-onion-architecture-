using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NavinoShop.WebApplication.Services;
using Users.Application.Bootstrapper;
using Users.Domain.User.Agg.IRepository;
using Users.Infrastructure.Persistence.Context;
using Users.Infrastructure.Persistence.Repository;

namespace Users.Infrastructure.Bootstrapper
{
    public class UserInfrastructureBootstrapper
    {
        public static void Config(IServiceCollection services, string connectionString)
        {
            UserApplicationBootstrapper.Config(services);
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserAddressRepository, UserAddressRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();


            services.AddDbContext<UserContext>(option =>
            {
                option.UseSqlServer(connectionString);
            });
        }
    }
}
