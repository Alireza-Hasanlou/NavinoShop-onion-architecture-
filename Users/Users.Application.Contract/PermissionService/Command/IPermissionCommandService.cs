using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Shared.Application;

namespace Users.Application.Contract.PermissionService.Command
{
    public interface IPermissionCommandService
    {
        Task<OperationResult> Create(CreatePermissionCommand command);
        Task<OperationResult> Edit(EditPermissionCommand command);  
    }
}
