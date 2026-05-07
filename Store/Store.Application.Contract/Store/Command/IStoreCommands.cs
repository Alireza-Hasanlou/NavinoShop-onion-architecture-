using Shared.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Contract.Store.Command
{
    public interface IStoreCommands
    {
        Task<OperationResult> CreateAsync(CreateStoreCommandModel command);
        Task<EditStoreCommandModel> GetForEditAsync(int Id);
        Task<OperationResult> EditAsync(EditStoreCommandModel command);
    }
}
