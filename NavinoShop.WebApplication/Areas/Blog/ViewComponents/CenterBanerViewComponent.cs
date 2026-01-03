using Microsoft.AspNetCore.Mvc;
using Site.Application.Contract.BanerService.Query;

namespace NavinoShop.WebApplication.Areas.Blog.ViewComponents
{
    public class CenterBanerViewComponent : ViewComponent
    {
        private readonly IBanerQueryService _banerQueryService;

        public CenterBanerViewComponent(IBanerQueryService banerQueryService)
        {
            _banerQueryService = banerQueryService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var baners = await _banerQueryService.GetCenterBanerForBlog();
            return View(baners);
        }
    }
}
