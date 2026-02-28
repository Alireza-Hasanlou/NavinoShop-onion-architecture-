using Microsoft.Extensions.DependencyInjection;
using Users.Application.Contract.PermissionService.Command;
using Users.Application.Contract.RoleService.Command;
using Users.Application.Contract.UserAddressService.Command;
using Users.Application.Contract.UserService.Command;
using Users.Application.Services;

namespace Users.Application.Bootstrapper
{
    public class UserApplicationBootstrapper
    {
        public static void Config(IServiceCollection services)
        {
            services.AddTransient<IUserCommandService,UserService>();
            services.AddTransient<IUserAddressCommandService, UserAddressService>();
            services.AddTransient<IRoleCommandService, RoleService>();
            services.AddTransient<IPermissionCommandService, PermissionService>();  
        }
    }
}
