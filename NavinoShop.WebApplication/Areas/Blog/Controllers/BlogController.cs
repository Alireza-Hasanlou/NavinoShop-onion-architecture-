using Blogs.Application.Contract.BlogService.Command;
using Blogs.Application.Contract.BlogService.Query;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Query.Contract.UI.Blog;
using Query.Contract.UI.Comments;
using Shared.Domain.Enums;

namespace NavinoShop.WebApplication.Areas.Blog.Controllers
{
    [Area("Blog")]
    [IgnoreAntiforgeryToken]
    public class BlogController : Controller
    {
        private readonly IBlogUiQueryService _blogUiQueryService;
        private readonly IBlogQueryService _blogQueryService;
        private readonly IBlogCommandService _blogCommandService;
        private readonly ICommentsUiQueryService _commentsUiQueryService;

        public BlogController(IBlogUiQueryService blogUiQueryService,
            IBlogQueryService blogQueryService,
            IBlogCommandService blogCommandService)
        {
            _blogUiQueryService = blogUiQueryService;
            _blogQueryService = blogQueryService;
            _blogCommandService = blogCommandService;
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
        [Route("/Blogs/{slug?}/{pageId?}/{filter?}")]
        public async Task<IActionResult> Blogs(string? slug = "", int pageId = 1, string filter = "")
        {
            var blogs = await _blogUiQueryService.GetBlogsForUi(slug, pageId, filter);
            return View(blogs);
        }

        [Route("/Blog/{Slug}")]
        public async Task<IActionResult> Blog(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return NotFound();
            var model = await _blogUiQueryService.GetBlogForUi(slug);
            if (model == null)
                return NotFound();
            await _blogCommandService.IncreaseVisitCountAsync(model.Id);
            return View(model);
        }
        [HttpGet]
        public async Task<JsonResult> GetBlogsComments(int pageId = 1, int ownerId = 0)
        {
            var comments = await _commentsUiQueryService.GetCommments(ownerId, CommentFor.مقاله, pageId);
            return Json(comments);

        }
    }
}
