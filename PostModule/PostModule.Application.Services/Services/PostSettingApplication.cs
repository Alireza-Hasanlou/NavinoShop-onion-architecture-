using PostModule.Application.Contract.PostSettingApplication.Command;
using PostModule.Domain.SettingAgg;
using Shared.Application;
using Shared.Application.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostModule.Application.Services
{
    internal class PostSettingApplication : IPostSettingApplication
    {
        private readonly IPostSettingRepository _postSettingRepository;
        public PostSettingApplication(IPostSettingRepository postSettingRepository)
        {
            _postSettingRepository = postSettingRepository;
        }
        public async Task<UpsertPostSetting> GetForUpsert() =>
           await _postSettingRepository.GetForUpsert();

        public async Task<OperationResult> Upsert(UpsertPostSetting command)
        {
            PostSetting setting = await _postSettingRepository.GetSingle();
            setting.Edit(command.PackageTitle, command.PackageDescription,command.ApiDescription);
            if (await _postSettingRepository.SaveAsync()) return new(true);
            return new OperationResult(false,ValidationMessages.SystemErrorMessage,nameof(command.PackageTitle));
        }
    }
}
