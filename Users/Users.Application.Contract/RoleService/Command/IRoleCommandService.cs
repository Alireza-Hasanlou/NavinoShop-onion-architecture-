using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Shared.Application;

namespace Users.Application.Contract.RoleService.Command
{
    public interface IRoleCommandService
    {
       Task< OperationResult> CreateAsync(CreateRoleCommand command, List<int> permissions);
        Task<OperationResult> EditAsync(EditRoleCommand command, List<int> permissions);
        Task<OperationResult> EditUserRoleAsync(int userId, List<int> roles);
       

    }
}
