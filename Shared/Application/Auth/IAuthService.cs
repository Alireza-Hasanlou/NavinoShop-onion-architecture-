using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Application.Auth
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(AuthModel command);
        Task LogoutAsync();
        int GetLoginUserId();
        string GetLoginUserAvatar();
        string GetLoginUserFullName();
        string GetLoginUserMobile();
        bool IsUserLogin();

    }
}
