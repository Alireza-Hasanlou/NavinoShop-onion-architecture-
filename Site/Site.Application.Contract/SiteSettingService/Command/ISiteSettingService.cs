using Shared.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.Application.Contract.SiteSettingService.Command
{
    public interface ISiteSettingService
    {
       Task< OperationResult> Ubsert(UbsertSiteSetting command);
        Task<UbsertSiteSetting> GetForUbsert();
    }
}
