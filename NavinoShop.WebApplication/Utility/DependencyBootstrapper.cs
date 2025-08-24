using NavinoShop.WebApplication.Services;
using Shared.Application.Auth;
using Shared.Application.Service;

namespace NavinoShop.WebApplication.Utility
{
    public class DependencyBootstrapper
    {
        public static void Congig(IServiceCollection services)
        {
            services.AddScoped<IFileService, FileServices>();
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
