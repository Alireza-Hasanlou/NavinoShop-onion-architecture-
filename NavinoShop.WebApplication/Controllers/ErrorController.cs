using Microsoft.AspNetCore.Mvc;

namespace NavinoShop.WebApplication.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error/NotFound")]
        public IActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View("~/Views/Shared/NotFound.cshtml");
        }

        [Route("/Error/StatusCode404")]
        public IActionResult StatusCode404()
        {
            Response.StatusCode = 404;
            return View("~/Views/Shared/NotFound.cshtml");
        }
    }
}
