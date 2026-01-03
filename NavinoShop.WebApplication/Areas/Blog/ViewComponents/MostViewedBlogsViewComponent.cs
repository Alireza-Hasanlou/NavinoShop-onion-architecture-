using Blogs.Application.Contract.BlogService.Query;
using Microsoft.AspNetCore.Mvc;

namespace NavinoShop.WebApplication.Areas.Blog.ViewComponents
{
    public class MostViewedBlogsViewComponent:ViewComponent
    {

        private readonly IBlogQueryService _blogQueryService;

        public MostViewedBlogsViewComponent(IBlogQueryService blogQueryService)
        {
            _blogQueryService = blogQueryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var blogs = await _blogQueryService.GetMostViewedPostsAsync(10);

            return View(blogs);
        }
    }
}
