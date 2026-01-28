using Shared.Application;
using Users.Application.Contract.UserAddressService.Query;

namespace Users.Application.Contract.UserAddressService.Command
{
    public partial interface IUserAddressCommandService
    {
        Task<OperationResult> CreateAsync(CreateUserAddressCommand command, int userId);
        Task<OperationResult> DeleteAsync(int id);
        Task<OperationResult> EditAsync(UserAddressDto command);
        Task<UserAddressDto> GetAddressForEditAsync(int id);
    }
}
