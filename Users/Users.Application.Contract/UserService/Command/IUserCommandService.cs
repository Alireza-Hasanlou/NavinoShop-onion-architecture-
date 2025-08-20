using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Shared.Application;

namespace Users.Application.Contract.UserService.Command
{
    public interface IUserCommandService
    {
        Task<OperationResult> RegisterAsync(RegisterUserCommand command);
        Task<OperationResult> LoginAsync(LoginUserCommand command);
        Task<OperationResult> CreateAsync(CreateUserCommand command);
        Task<OperationResult> EditAsync(EditUserByAdminCommand command);
        Task<OperationResult> EditByUserAsync(EditUserByUserCommand command, int userId);
        Task<OperationResult> ChangePasswordAsync(ChangeUserPasswordCommand command);
        Task<OperationResult> ActivationChangeAsync(int id);
        Task<OperationResult> DeleteChangeAsync(int id);
        Task LogoutAsync();
    }

}
