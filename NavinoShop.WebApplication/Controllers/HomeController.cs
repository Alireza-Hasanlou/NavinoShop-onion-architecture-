using Emails.Application.Contract.EmailUserService.Command;
using Emails.Application.Contract.MessageUserService.Command;
using Microsoft.AspNetCore.Mvc;
using NavinoShop.WebApplication.Models;
using NavinoShop.WebApplication.Services;
using Query.Contract.Site.Page;
using Shared.Application.Auth;
using Site.Application.Contract.SitePageService.Query;
using Site.Application.Contract.SiteSettingService.Query;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Controllers
{
    [IgnoreAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMessageUserCommandService _messageUserCommandService;
        private readonly ISiteSettingQueryService _siteSettingQueryService;
        private readonly IAuthService _authService;
        private readonly IEmailUseCommandService _emailUseCommandService;
        private readonly ISitePageUiQueryService _sitePageUiQueryService;


        public HomeController(ILogger<HomeController> logger, IMessageUserCommandService messageUserCommandService,
            ISiteSettingQueryService siteSettingQueryService, IAuthService authService,
            IEmailUseCommandService emailUseCommandService,ISitePageUiQueryService pageUiQueryService)
        {
            _logger = logger;
            _messageUserCommandService = messageUserCommandService;
            _siteSettingQueryService = siteSettingQueryService;
            _authService = authService;
            _emailUseCommandService = emailUseCommandService;
            _sitePageUiQueryService = pageUiQueryService;
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
        [HttpPost]
        
        public async Task<JsonResult> AddUserEmail(string email)
        {
            var Userid =  _authService.GetLoginUserId();
            var model = new CreateEmailUserCommandModel { UserId = Userid, Email = email };
            var res = await _emailUseCommandService.Create(model);
            if (res.Success)
            {
                return new JsonResult(new { success = true });
            }
            return new JsonResult(new { success = false , message=res.Message});
        }
        [Route("/AboutUs")]
        public async Task<IActionResult> AboutUs()
        {
            var model= await _siteSettingQueryService.GetAboutUsForUi();
            return View(model);
        }
        [HttpGet]
        [Route("/page/{slug}")]
        public async Task<IActionResult> page(string slug)
        {
            if (string.IsNullOrEmpty(slug)) return NotFound();
            var page =  await _sitePageUiQueryService.GetPageAsync(slug);
            if (page == null) return NotFound();
            return View(page);
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
