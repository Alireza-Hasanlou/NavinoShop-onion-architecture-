using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Query.Contract.Admin.Comment;
using Shared.Domain.Enums;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Comments
{
    public class IndexModel : PageModel
    {
        private readonly ICommentAdminQuery _commentQueryService;

        public IndexModel(ICommentAdminQuery commentQueryService)
        {
            _commentQueryService = commentQueryService;
        }
        public CommentForAdminPaging Comments { get; set; }
        public async Task<IActionResult> OnGet(int pageId, int take, string filter, int ownerId,
            CommentFor commentFor, CommentStatus commentStatus, int? parentId)
        {
            Comments = await _commentQueryService.GetForAdmin(pageId,take,filter,ownerId,commentFor,commentStatus,parentId);
            return Page();
        }
    }
}
