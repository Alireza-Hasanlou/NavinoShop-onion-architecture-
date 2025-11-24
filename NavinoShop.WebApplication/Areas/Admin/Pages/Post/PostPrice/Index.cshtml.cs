using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PostModule.Application.Contract.PostApplication;
using PostModule.Application.Contract.PostQuery;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Post.PostPrice
{
    public class IndexModel : PageModel
    {
        public IPostQuery _postQuery { get; set; }

        public IndexModel(IPostQuery postQuery)
        {
            _postQuery = postQuery;
        }
        public PostAdminDetailQueryModel PostDetails { get; set; }
        public async Task OnGet(int id)
        {
            PostDetails = await _postQuery.GetPostDetails(id);
        }
    }
}
