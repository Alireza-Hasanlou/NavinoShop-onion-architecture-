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
        Task<OperationResult> CreateAsync(CreateSiteServiceCommnadModel commmand);
        Task<OperationResult> EditAsync(EditSiteServiceCommandModel commmand);
        Task<OperationResult> ActivationChangeAsync(int id);
        Task<EditSiteServiceCommandModel> GetForEditAsync(int id);
    }
}
