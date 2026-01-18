using Shared.Application;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostModule.Application.Contract.UserPostApplication.Command;

public interface IPackageCommandService
{
    Task<OperationResult> Create(CreatePackage command);
    Task<OperationResult> Edit(EditPackage command);
    Task<EditPackage> GetForEdit(int id);
    Task<bool> ActivationChange(int id);

}
