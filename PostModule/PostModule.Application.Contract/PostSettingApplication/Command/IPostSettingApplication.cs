using Shared.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostModule.Application.Contract.PostSettingApplication.Command
{
    public interface IPostSettingApplication
    {
        Task<UpsertPostSetting> GetForUpsert();
        Task<OperationResult> Upsert(UpsertPostSetting command);
    }
}
