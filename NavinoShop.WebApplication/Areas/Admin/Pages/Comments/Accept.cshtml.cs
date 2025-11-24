using Comments.Application.Contract.CommentService.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Comments
{
    public class AcceptModel : PageModel
    {
        private readonly ICommentCommandService _commentCommandService;

        public AcceptModel(ICommentCommandService commentCommandService)
        {
            _commentCommandService = commentCommandService;
        }

        public async Task<JsonResult> OnGet(long id)
        {

            if (id < 1)
                return new JsonResult(new { success = false });

            var result = await _commentCommandService.AcceptedComment(id);
            if (result.Success)
                return new JsonResult(new { success = true });

            return new JsonResult(new { success = false });
        }
    }
}

