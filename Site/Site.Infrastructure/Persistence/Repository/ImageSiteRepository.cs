using Shared.Insfrastructure;
using Site.Domain.SiteImageAgg;
using Site.Infrastructure.Persistence.Context;

namespace Site.Infrastructure.Persistence.Repository;

internal class ImageSiteRepository : GenericRepository<SiteImage, int>, IImageSiteRepository
{
	private readonly SiteContext _context;

	public ImageSiteRepository(SiteContext context) : base(context)
	{
		_context = context;
	}
}