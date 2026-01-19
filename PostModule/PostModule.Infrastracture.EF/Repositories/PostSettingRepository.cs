
using PostModule.Domain.SettingAgg;
using PostModule.Application.Contract.PostSettingApplication.Command;
using PostModule.Infrastracture.Context;
using Shared.Insfrastructure;
using System.Threading.Tasks;

namespace PostModule.Infrastracture.Repositories;

internal class PostSettingRepository : GenericRepository<PostSetting, int>, IPostSettingRepository
{
    private readonly PostContext _context;
    public PostSettingRepository(PostContext context) : base(context)
    {
        _context = context;
    }

    public async Task<UpsertPostSetting> GetForUpsert()
    {
        var s = await GetSingle();
        return new()
        {
            PackageDescription = s.PackageDescription,
            PackageTitle = s.PackageTitle,
            ApiDescription = s.ApiDescription
        };
    }

    public async Task<PostSetting> GetSingle()
    {
        var setting = _context.PostSettings.SingleOrDefault();
        if(setting == null)
        {
            setting = new("", "","");
          await CreateAsync(setting);
        }
        return setting;
    }
}