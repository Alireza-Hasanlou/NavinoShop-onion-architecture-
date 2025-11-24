using Shared.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostModule.Application.Contract.StateApplication
{
    public interface IStateApplication
    {
        Task<OperationResult> CreateAsync(CreateStateModel command);
        Task<OperationResult> EditAsync(EditStateModel command);
        Task<List<StateViewModel>> GetAll();
        Task<EditStateModel> GetStateForEditAsync(int id);
        Task<bool> ExistTitleForCreateAsync(string title);
        Task<bool> ExistTitleForEditAsync(string title, int id);
        Task<bool> ChangeStateCloseAsync(int id, List<int> stateCloses);
    }
}
