using Blogs.Application.Contract.BlogService.Query;
using Microsoft.AspNetCore.Mvc;

namespace NavinoShop.WebApplication.Areas.Blog.ViewComponents
{
    public class TrendingNowViewComponent : ViewComponent
    {
        private readonly IBlogQueryService _blogQueryService;

        public TrendingNowViewComponent(IBlogQueryService blogQueryService)
        {
            _blogQueryService = blogQueryService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var blogs = await _blogQueryService.GetLastBlogsAsync(5);

            return View(blogs);
        }
    }
}
