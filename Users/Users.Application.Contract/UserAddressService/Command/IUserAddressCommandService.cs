using Shared.Application;

namespace Users.Application.Contract.UserAddressService.Command
{
    public partial interface IUserAddressCommandService
    {
        Task<OperationResult> CreateAsync(CreateUserAddressCommand command, int userId);
        Task<OperationResult> DeleteAsync(int id);
    }
}
