using Shared.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.Application.Contract.SitePageService.Command
{
    public interface ISitePageCommandService
    {
        Task<OperationResult> CreateAsync(CreateSitePageCommnadModel command);
        Task<OperationResult> EditAsync(EditSitePageCommandModel command);
        Task<EditSitePageCommandModel> GetForEditAsync(int id);
        Task<OperationResult> ActivationChangeAsync(int id);
    }
}
