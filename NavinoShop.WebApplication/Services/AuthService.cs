using Shared.Application.Auth;

namespace NavinoShop.WebApplication.Services
{
    internal class AuthService : IAuthService
    {
        public string GetLoginUserAvatar()
        {
            throw new NotImplementedException();
        }

        public string GetLoginUserFullName()
        {
            throw new NotImplementedException();
        }

        public int GetLoginUserId()
        {
            throw new NotImplementedException();
        }

        public bool IsUserLogin()
        {
            throw new NotImplementedException();
        }

        public Task<bool> LoginAsync(AuthModel command)
        {
            throw new NotImplementedException();
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }
    }
}
