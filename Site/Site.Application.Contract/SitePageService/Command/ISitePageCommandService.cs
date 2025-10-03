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
        Task<OperationResult> Create(CreateSitePageCommnadModel command);
        Task<OperationResult> Edit(EditSitePageCommandModel command);
        Task<EditSitePageCommandModel> GetForEdit(int id);
        Task<OperationResult> ActivationChange(int id);
    }
}
