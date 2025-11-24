using Comments.Application.Contract.CommentService.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Comments
{
    public class RejectModel : PageModel
    {
        private readonly ICommentCommandService _commentCommandService;

        public RejectModel(ICommentCommandService commentCommandService)
        {
            _commentCommandService = commentCommandService;
        }

        public async Task<JsonResult> OnGet(long Id, string why)
        {

            if (Id < 1 || string.IsNullOrEmpty(why))
                return new JsonResult(new { success = false });

            var result = await _commentCommandService.Reject(new RejectCommentCommandModel { Id = Id, Why = why });
            if (result.Success)
                return new JsonResult(new { success = true });

            return new JsonResult(new { success = false });
        }
    }
}
