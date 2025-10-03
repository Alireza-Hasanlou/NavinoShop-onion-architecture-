using Shared.Application;


namespace Users.Application.Contract.RoleService.Command
{
    public interface IRoleCommandService
    {
        Task<OperationResult> CreateAsync(CreateRoleCommand command, List<int> permissions);
        Task<OperationResult> EditAsync(EditRoleCommand command, List<int> permissions);
        Task<OperationResult> DeleteAsync(int id);
        Task<OperationResult> EditUserRoleAsync(int userId, List<int> roles);


    }
}
