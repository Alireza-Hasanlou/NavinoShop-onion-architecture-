
using Microsoft.EntityFrameworkCore;
using Shared.Insfrastructure;
using Site.Application.Contract.SitePageService.Command;
using Site.Application.Contract.SiteServiceService.Command;
using Site.Domain.SiteServiceAgg;
using Site.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.Infrastructure.Persistence.Repository;

internal class SiteServiceRepository : GenericRepository<SiteService, int> , ISiteServiceRepository
{
    private readonly SiteContext _context;

    public SiteServiceRepository(SiteContext context) : base(context)
    {
        _context = context;
    }

    public async Task<EditSiteServiceCommandModel> GetForEdit(int id)
    {
     return await  _context.SiteServices.Select(s => new EditSiteServiceCommandModel
      {
            ImageAlt = s.ImageAlt,
            Id = s.Id,
            ImageFile = null,
            ImageName = s.ImageName,
            Title = s.Title
        }).SingleOrDefaultAsync(s => s.Id == id);
    }

}
