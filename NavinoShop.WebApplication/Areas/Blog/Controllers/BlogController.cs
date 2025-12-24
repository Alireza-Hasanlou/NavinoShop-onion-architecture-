using Microsoft.AspNetCore.Mvc;

namespace NavinoShop.WebApplication.Areas.Blog.Controllers
{
    [Area("Blog")]
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
