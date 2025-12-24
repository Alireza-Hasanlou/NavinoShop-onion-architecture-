using Blogs.Application.Contract.BlogService.Query;
using Microsoft.AspNetCore.Mvc;

namespace NavinoShop.WebApplication.Areas.Blog.ViewComponents
{
    public class FeaturedpostsViewComponent:ViewComponent
    {
        private readonly IBlogQueryService _blogQueryService;

        public FeaturedpostsViewComponent(IBlogQueryService blogQueryService)
        {
            _blogQueryService = blogQueryService;
        }

        public async Task<IViewComponentResult>InvokeAsync()
        {
            
            return View();
        }
    }
}
