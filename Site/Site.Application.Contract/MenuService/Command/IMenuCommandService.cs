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
        Task<OperationResult> Create(CreateMenuCommandModel command);
        Task<CreateSubMenuCommandModel> GetForCreate(int parentId);
        Task<OperationResult> CreateSub(CreateSubMenuCommandModel command);
        Task<OperationResult> Edit(EditMenuCommandModel command);
        Task<EditMenuCommandModel> GetForEdit(int id);
        Task<OperationResult> ActivationChange(int id);
    }
}
