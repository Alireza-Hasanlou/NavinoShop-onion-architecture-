using Emails.Application.Contract.MessageUserService.Command;
using Microsoft.AspNetCore.Mvc;
using NavinoShop.WebApplication.Models;
using NavinoShop.WebApplication.Services;
using Shared.Application.Auth;
using Site.Application.Contract.SiteSettingService.Query;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMessageUserCommandService _messageUserCommandService;
        private readonly ISiteSettingQueryService _siteSettingQueryService;
        private readonly IAuthService _authService;

        public HomeController(ILogger<HomeController> logger, IMessageUserCommandService messageUserCommandService,
            ISiteSettingQueryService siteSettingQueryService, IAuthService authService)
        {
            _logger = logger;
            _messageUserCommandService = messageUserCommandService;
            _siteSettingQueryService = siteSettingQueryService;
            _authService = authService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Route("/ContactUs")]
        public async Task<IActionResult> ContactUs()
        {
            ViewData["ContactInformation"] = await _siteSettingQueryService.GetContactData();
            return View();
        }
        [Route("/ContactUs")]
        [HttpPost]
        public async Task<IActionResult> ContactUs(CreateMessageUserCommandModel MessageUser)
        {
            ViewData["ContactInformation"] = await _siteSettingQueryService.GetContactData();
            if (!ModelState.IsValid)
                return View();

            MessageUser.UserId = _authService.GetLoginUserId();
            var result = await _messageUserCommandService.CreateAsync(MessageUser);
            if (result.Success)
                ViewData["Success"] = true;

            return View();
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
