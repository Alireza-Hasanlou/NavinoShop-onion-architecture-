using Shared.Domain.Enums;


namespace Site.Application.Contract.BanerService.Query
{
    public interface IBanerQueryService
    {
       Task< List<BanerForAdminQueryModel>> GetAllForAdmin();
        Task<List<BanerForUiQueryModel>> GetForUi(int count, BanerState state);
    }
}
