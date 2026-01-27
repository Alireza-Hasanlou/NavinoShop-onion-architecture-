using Microsoft.AspNetCore.Authentication;
using Shared.Application.Auth;
using System.Security.Claims;

namespace NavinoShop.WebApplication.Services
{
    internal class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string GetLoginUserAvatar()
        {
            var avatarClaim = _contextAccessor.HttpContext.User.Claims
                 .FirstOrDefault(c => c.Type == "Avatar");
            return avatarClaim?.Value ?? string.Empty;
        }

        public string GetLoginUserEmail()
        {

            var EmailClaim = _contextAccessor.HttpContext.User.Claims
                 .FirstOrDefault(c => c.Type == "Email");
            return EmailClaim?.Value ?? string.Empty;
        }

        public string GetLoginUserFullName()
        {
            var fullNameClaim = _contextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Name);
            return fullNameClaim?.Value ?? string.Empty;
        }

        public int GetLoginUserId()
        {
            var userIdClaim = _contextAccessor.HttpContext.User.Claims
                 .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return 0;
        }

        public string GetLoginUserMobile()
        {
            var mobileClaim = _contextAccessor.HttpContext.User.Claims
                 .FirstOrDefault(c => c.Type == "Mobile");  
            return mobileClaim?.Value ?? string.Empty;  
        }

        public bool IsUserLogin()
        {
            return _contextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public async Task<bool> LoginAsync(AuthModel command)
        {
            try
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, command.UserId.ToString()),
            new Claim(ClaimTypes.Name, command.FullName ?? string.Empty),
            new Claim("Avatar", command.Avatar ?? string.Empty),
            new Claim("Mobile", command.Mobile ?? string.Empty),
            new Claim("Email", command.Email ?? string.Empty)
        };

                var identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);


                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(15)
                };

                 await _contextAccessor.HttpContext.SignInAsync("Cookies", principal, authProperties);

                return true;
            }
            catch (Exception)
            {


                return false;
            }
        }


        public async Task LogoutAsync()
        {
            await _contextAccessor.HttpContext.SignOutAsync("Cookies");
        }
    }
}
