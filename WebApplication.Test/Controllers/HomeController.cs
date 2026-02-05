using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Diagnostics;
using WebApplication.Test.Interfaces;
using WebApplication.Test.Models;

namespace WebApplication.Test.Controllers
{
    [IgnoreAntiforgeryToken]
    public partial class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostApiService _postApiService;

        public HomeController(ILogger<HomeController> logger, IPostApiService postApiService)
        {
            _logger = logger;
            _postApiService = postApiService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> States()
        {
            var response = await _postApiService.States();
            return Json(response);
        }
        public async Task<JsonResult> Cities(int stateId)
        {
            var response= await _postApiService.Cities(stateId);
            return Json(response);
        }
        [HttpPost]
        public async Task<JsonResult> Calculate(PostPriceModel model)
        {
            var response= await _postApiService.Calculate(model);
            return Json(response);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
