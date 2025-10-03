using Shared;

namespace Site.Application.Contract.ImageSiteService.Query
{
    public class ImageAdminPaging : BasePaging
    {
        public string Filter { get; set; }
        public List<ImageSiteAdminQueryModel> Images { get; set; }
	}
}
