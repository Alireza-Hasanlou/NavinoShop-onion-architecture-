using Shared.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.Application.Contract.SiteServiceService.Command
{
    public interface ISiteServiceCommandService
    {
        Task<OperationResult> Create(CreateSiteServiceCommnadModel commmand);
        Task<OperationResult> Edit(EditSiteServiceCommandModel commmand);
        Task<OperationResult> ActivationChange(int id);
        Task<EditSiteServiceCommandModel> GetForEdit(int id);
    }
}
