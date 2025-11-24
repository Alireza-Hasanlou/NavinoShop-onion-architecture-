using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Query.Contract.Admin.Comment;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Comments
{
    public class UnseenModel : PageModel
    {
        private readonly ICommentAdminQuery _commentQuery;

        public UnseenModel(ICommentAdminQuery commentQuery)
        {
            _commentQuery = commentQuery;
        }
        public List<CommentAdminQueryModel> UnseenComments { get; set; }
        public async Task OnGet()
        {
            UnseenComments= await _commentQuery.GetAllUnseenCommentsForAdmin(); 
        }
    }
}
