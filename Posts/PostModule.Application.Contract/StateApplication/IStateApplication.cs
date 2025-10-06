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
        Task<OperationResult> Create(CreateStateModel command);
        Task<OperationResult> Edit(EditStateModel command);
        Task<List<StateViewModel>> GetAll();
        Task<EditStateModel> GetStateForEdit(int id);
        Task<bool> ExistTitleForCreate(string title);
        Task<bool> ExistTitleForEdit(string title, int id);
        Task<bool> ChangeStateClose(int id, List<int> stateCloses);
    }
}
