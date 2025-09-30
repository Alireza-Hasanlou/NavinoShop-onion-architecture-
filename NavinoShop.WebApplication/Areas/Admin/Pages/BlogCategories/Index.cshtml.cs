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
        public async Task<IActionResult> OnGet(int Id )
        {
            if (Id > 0 && await _blogCategoryQueryService.CheckCategoryHaveParentAsync(Id))
                return RedirectToPage("index");

            BlogCategories = await _blogCategoryQueryService.GetCategoriesForAdminAsync(Id);
            return Page();
        }
    }
}
