using Blogs.Application.Contract.BlogService.Query;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace NavinoShop.WebApplication.Areas.Blog.Controllers
{
    [Area("Blog")]
    [IgnoreAntiforgeryToken]
    public class BlogController : Controller
    {
        private readonly IBlogQueryService _blogQueryService;

        public BlogController(IBlogQueryService blogQueryService)
        {
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
    }
}
