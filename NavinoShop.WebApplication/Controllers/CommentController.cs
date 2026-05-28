using Comments.Application.Contract.CommentService.Command;
using Microsoft.AspNetCore.Mvc;
using Query.Contract.UI.Comments;
using Shared.Application.Auth;
using Shared.Domain.Enums;

namespace NavinoShop.WebApplication.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentsUiQueryService _commentsUiQueryService;
        private readonly ICommentCommandService _commentCommandService;
        private readonly IAuthService _authService;

        public CommentController(ICommentsUiQueryService commentsUiQueryService,
            ICommentCommandService commentCommandService, IAuthService authService)
        {
            _commentsUiQueryService = commentsUiQueryService;
            _commentCommandService = commentCommandService;
            _authService = authService;
        }

        [HttpGet]
        public async Task<JsonResult> GetComments(int ProductId, int pageId = 1)
        {
            var comments = await _commentsUiQueryService.GetCommments(ProductId, CommentFor.محصول, pageId);
            return Json(comments);
        }

        [HttpPost]
        public async Task<JsonResult> AddComment([FromBody] CreateCommentCommandModel model)
        {
            var userId = _authService.GetLoginUserId();
            model.UserId = userId;
            model.For = CommentFor.محصول;
            var res = await _commentCommandService.Create(model);
            if (res.Success)
                return new JsonResult(new { success = true, message = "نظر شما دریارفت شد و پس از بازبینی منتشر خواهد شد" });
            return new JsonResult(new { success = false, message = res.Message });
        }

        [HttpPost]
        public async Task<JsonResult> AddReply([FromBody] CreateCommentCommandModel model)
        {
            var userId = _authService.GetLoginUserId();
            model.FullName = _authService.GetLoginUserFullName();
            model.UserId = userId;
            model.For = CommentFor.محصول;
            var res = await _commentCommandService.Create(model);
            if (res.Success)
                return new JsonResult(new { success = true, message = "پاسخ شما دریارفت شد و پس از بازبینی منتشر خواهد شد" });
            return new JsonResult(new { success = false, message = res.Message });
        }
    }
}
