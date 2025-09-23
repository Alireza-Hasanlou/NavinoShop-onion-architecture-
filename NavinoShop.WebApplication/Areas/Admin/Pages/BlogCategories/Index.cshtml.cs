using Blogs.Application.Contract.BlogCategoryService.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.BlogCategories
{
    public class IndexModel : PageModel
    {
        private readonly IBlogCategoryQueryService _blogCategoryQueryService;

        public IndexModel(IBlogCategoryQueryService blogCategoryQueryService)
        {
            _blogCategoryQueryService = blogCategoryQueryService;
        }
        public BlogCategoryAdminPageQueryModel BlogCategories { get; set; }
        public async Task<IActionResult> OnGet(int parentId = 0)
        {
            if (parentId > 0 && await _blogCategoryQueryService.CheckCategoryHaveParentAsync(parentId))
                return NotFound();

            BlogCategories = await _blogCategoryQueryService.GetCategoriesForAdminAsync(parentId);
            return Page();
        }
    }
}
