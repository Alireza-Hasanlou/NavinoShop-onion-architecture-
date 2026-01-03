using Microsoft.AspNetCore.Mvc;
using Site.Application.Contract.BanerService.Query;

namespace NavinoShop.WebApplication.Areas.Blog.ViewComponents
{
    public class LeftSideBanerViewComponent : ViewComponent
    {
        private readonly IBanerQueryService _banerQueryService;

        public LeftSideBanerViewComponent(IBanerQueryService banerQueryService)
        {
            _banerQueryService = banerQueryService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var baners = await _banerQueryService.GetLeftSideBanerForBlog();
            return View(baners);
        }
    }
}
