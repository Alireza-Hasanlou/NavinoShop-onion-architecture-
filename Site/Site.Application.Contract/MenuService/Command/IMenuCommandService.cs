using Shared.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.Application.Contract.MenuService.Command
{
    public interface IMenuCommandService
    {
        Task<OperationResult> CreateAsync(CreateMenuCommandModel command);
        Task<CreateSubMenuCommandModel> GetForCreateAsync(int parentId);
        Task<OperationResult> CreateSubAsync(CreateSubMenuCommandModel command);
        Task<OperationResult> EditAsync(EditMenuCommandModel command);
        Task<EditMenuCommandModel> GetForEditAsync(int id);
        Task<OperationResult> ActivationChangeAsync(int id);
    }
}
