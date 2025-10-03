namespace Site.Application.Contract.SiteServiceService.Query;
public interface ISiteServiceQuery
{
    Task<List<SiteServiceAdminQueryModel>> GetAllForAdmin();
    Task<List<SiteServiceUIQueryModel>> GetAllForUI();
}
