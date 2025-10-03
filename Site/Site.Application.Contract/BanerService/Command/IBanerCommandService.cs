
using Shared.Application;

namespace Site.Application.Contract.BanerService.Command
{
    public interface IBanerCommandService
    {
       Task< OperationResult> Create(CreateBanerCommandModel command);
        Task<OperationResult> Edit(EditBanerCommandModel command);
        Task<OperationResult> ActivationChange(int id);
        Task<EditBanerCommandModel> GetForEdit(int id);

    }
}
