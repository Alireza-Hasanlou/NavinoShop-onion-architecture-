namespace Site.Application.Contract.SitePageService.Query
{
    public interface ISitePageQueryService
    {
        Task<List<SitePageAdminQueryModel>> GetAllForAdmin();
    }
}
