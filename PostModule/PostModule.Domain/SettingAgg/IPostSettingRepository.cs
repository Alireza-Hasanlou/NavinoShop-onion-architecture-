using PostModule.Application.Contract.PostSettingApplication.Command;
using Shared.Domain;

namespace PostModule.Domain.SettingAgg
{
    public interface IPostSettingRepository : IGenericRepository<PostSetting, int>
    {
        Task<UbsertPostSetting> GetForUbsert();
        Task<PostSetting> GetSingle();
    }
}
