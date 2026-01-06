using Blogs.Application.Contract.BlogService.Query;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Query.Contract.UI.Blog;

namespace NavinoShop.WebApplication.Areas.Blog.Controllers
{
    [Area("Blog")]
    [IgnoreAntiforgeryToken]
    public class BlogController : Controller
    {
        private readonly IBlogUiQueryService _blogUiQueryService;
        private readonly IBlogQueryService _blogQueryService;

        public BlogController(IBlogUiQueryService blogUiQueryService, IBlogQueryService blogQueryService)
        {
            _blogUiQueryService = blogUiQueryService;
            _blogQueryService = blogQueryService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetBestBlogs()
        {
            var model = await _blogQueryService.GetBestBlogs();
            return Json(model);
        }
        [Route("/Blogs/{slug?}")]

        public async Task<IActionResult> Blogs(string? slug, int pageId = 1, string filter = "")
        {
            var blogs = await _blogUiQueryService.GetBlogsForUi(slug, pageId, filter);
            return View(blogs);
        }
    }
}
