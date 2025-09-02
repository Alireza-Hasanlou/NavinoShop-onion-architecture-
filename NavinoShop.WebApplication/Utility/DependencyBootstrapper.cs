using Microsoft.AspNetCore.Authentication.Cookies;
using NavinoShop.WebApplication.Services;
using Shared.Application.Auth;
using Shared.Application.Service;

namespace NavinoShop.WebApplication.Utility
{
    public class DependencyBootstrapper
    {
        public static void Congig(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie("Cookies", option =>
            {
                option.LoginPath = "/Account/Login";
                option.LogoutPath = "/Account/Logout";
                option.AccessDeniedPath = "/Account/AccessDenied";
                option.ExpireTimeSpan = TimeSpan.FromDays(20);
            });
            services.AddScoped<IFileService, FileServices>();
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
