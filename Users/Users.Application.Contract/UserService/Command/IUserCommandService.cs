using Shared.Application;
using Shared.Application.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Contract.UserService.Query;

namespace Users.Application.Contract.UserService.Command
{
    public interface IUserCommandService
    {
        Task<OperationResult> RegisterAsync(RegisterUserCommand command);
        Task<OperationResult> LoginAsync(LoginUserCommand command);
        Task<OperationResult> CreateAsync(CreateUserCommand command);
        Task<OperationResult> EditByAdminAsync(EditUserByAdminDto command);
        Task<EditUserByUserDto> GetForEditByUserAsync(int userId);
        Task<OperationResult> EditByUserAsync(EditUserByUserCommand command, int userId);
        Task<OperationResult> ChangePasswordAsync(ChangeUserPasswordCommand command);
        Task<OperationResult> ActivationChangeAsync(int id);
        Task<OperationResult> DeleteChangeAsync(int id);
        Task LogoutAsync();
    }

}
