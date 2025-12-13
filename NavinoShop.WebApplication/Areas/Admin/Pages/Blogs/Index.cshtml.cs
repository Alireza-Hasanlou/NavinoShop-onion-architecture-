using Blogs.Application.Contract.BlogCategoryService.Query;
using Blogs.Application.Contract.BlogService.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Blogs
{
    public class IndexModel : PageModel
    {
        private readonly IBlogQueryService _blogQueryService;
        private readonly IBlogCategoryQueryService _blogCategoryQueryService;

        public IndexModel(IBlogQueryService blogQueryService, IBlogCategoryQueryService blogCategoryQueryService)
        {
            _blogQueryService = blogQueryService;
            _blogCategoryQueryService = blogCategoryQueryService;
        }

        public AdminBlogPageQueryModel blogs { get; set; }
        public async Task<IActionResult> OnGet(int Id)
        {
            blogs = await _blogQueryService.GetBlogsForAdmin(Id);
            return Page();  
        }
    }
}
