using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Users.Application.Bootstrapper;
using Users.Domain.User.Agg.IRepository;
using Users.Domain.WalletAgg;
using Users.Infrastructure.Persistence.Context;
using Users.Infrastructure.Persistence.Repository;

namespace Users.Infrastructure.Bootstrapper
{
    public class UserInfrastructureBootstrapper
    {
        public static void Config(IServiceCollection services, string connectionString)
        {
            UserApplicationBootstrapper.Config(services);
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IUserAddressRepository, UserAddressRepository>();
            services.AddTransient<IPermissionRepository, PermissionRepository>();
            services.AddTransient<IWalletRepository, WalletRepository>();

            services.AddDbContext<UserContext>(option =>
            {
                option.UseSqlServer(connectionString);
            });
        }
    }
}
