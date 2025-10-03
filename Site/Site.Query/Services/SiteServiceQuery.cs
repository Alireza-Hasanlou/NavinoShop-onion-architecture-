using Microsoft.EntityFrameworkCore;
using Shared.Application;
using Site.Application.Contract.SiteServiceService.Query;
using Site.Domain.SiteServiceAgg;
using System.Threading.Tasks;

namespace Site.Query.Services;

internal class SiteServiceQuery : ISiteServiceQuery
{
    private readonly ISiteServiceRepository _siteServiceRepository;

    public SiteServiceQuery(ISiteServiceRepository siteServiceRepository)
    {
        _siteServiceRepository = siteServiceRepository;
    }

    public async Task<List<SiteServiceAdminQueryModel>> GetAllForAdmin()
    {

        return await _siteServiceRepository.GetAll().Select(s => new SiteServiceAdminQueryModel
          (s.Id, s.Title, FileDirectories.ServiceImageDirectory100 + s.ImageName, s.ImageAlt, s.CreateDate.ToPersainDate(), s.Active)).ToListAsync();

    }

    public async Task<List<SiteServiceUIQueryModel>> GetAllForUI()
    {

        return await _siteServiceRepository.GetAllBy(s => s.Active).Select(s => new SiteServiceUIQueryModel
         (s.Id, s.Title, FileDirectories.ServiceImageDirectory + s.ImageName, s.ImageAlt)).ToListAsync();
    }





}
