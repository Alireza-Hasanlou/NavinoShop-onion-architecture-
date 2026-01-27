using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostModule.Application.Contract.PostApplication;
using PostModule.Application.Contract.UserPostApplication.Command;
using PostModule.Domain.UserPostAgg;
using Query.Contract.UI.UserPanel.PostOrder;
using Shared.Application.Auth;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Route("/profile/[action]/{id?}")]
    [Authorize]
    public class PostOrderController : Controller
    {
        private readonly IPostOrderQueryService _postOrderQueryService;
        private readonly IUserPostApplication _userPostApplication;
        private readonly IAuthService _authService;

        public PostOrderController(IPostOrderQueryService postOrderQueryService,
            IUserPostApplication userPostApplication, IAuthService authService)
        {
            _postOrderQueryService = postOrderQueryService;
            _userPostApplication = userPostApplication;
            _authService = authService;
        }
    
        public async Task<IActionResult> Orders()
        {
            int UserId = _authService.GetLoginUserId();
            var orders = await _postOrderQueryService.GetPostOrderForUserPanelAsync(UserId);
            return View(orders);
        }

        public async Task<IActionResult> ApiCart()
        {
            int UserId = _authService.GetLoginUserId();
            var res= await _userPostApplication.GetPostOrderNotPaymentForUser(UserId);
            return View(res);
        }
    
        public async Task<IActionResult> Create(int packageId)
        {
            if (packageId <1 )
                return Redirect("/Packages");
            var userId= _authService.GetLoginUserId();
            var model = await _userPostApplication.GetCreatePostModelAsync(userId, packageId);
            if(model == null)
                return Redirect("/Packages");
            var res = await _userPostApplication.CreatePostOrderAsync(model);
            if (res)
            {
                ViewData["success"] = true;     
                return Redirect("/PostOrder/Cart");
            }
            return Redirect("/Packages");
        }
  
        public async Task<IActionResult> Payment()
        {
            var UserId = _authService.GetLoginUserId();
            var package = await _userPostApplication.GetPostOrderNotPaymentForUser(UserId);
            if (package==null)
            {
                ViewData["existPackage"] = "پکیج مورد نظر یافت نشد!";
            }
            var res = await _userPostApplication.PaymentPostOrderAsync(new PaymentPostModel(UserId, 1, package.Price));
            if (res)
            {
                ViewData["success"] = "پرداخت با موفقیت انجام شد ";
            } 
            return Redirect("/PostOrder/Orders");
        }

        [HttpGet]
        [Route("/PostOrder/TestPostApi")] 
        public async Task<IActionResult> TestPostApi()
        {
            var userId=  _authService.GetLoginUserId();
            var userPost= await _userPostApplication.GetUserPostModelForPanel(userId);
            return View(userPost);
        }
    }
}
