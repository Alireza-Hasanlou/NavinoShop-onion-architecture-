
using Shared.Application;

namespace Site.Application.Contract.BanerService.Command
{
    public interface IBanerCommandService
    {
       Task< OperationResult> CreateAsync(CreateBanerCommandModel command);
        Task<OperationResult> EditAsync(EditBanerCommandModel command);
        Task<OperationResult> ActivationChangeAsync(int id);
        Task<EditBanerCommandModel> GetForEditAsync(int id);

    }
}
