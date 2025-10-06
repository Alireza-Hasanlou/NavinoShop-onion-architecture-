using Shared.Application;
using Shared.Domain.Enums;

namespace Seos.Application.Contract.SeoService.Command
{
    public interface ISeoCommandService
    {
        Task<OperationResult> UbsertSeo(CreateSeoCommandModel command);
        CreateSeoCommandModel GetSeoForEdit(int ownerId, WhereSeo where);
    }
}
