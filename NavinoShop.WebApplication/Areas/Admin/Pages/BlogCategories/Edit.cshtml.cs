using Blogs.Application.Contract.BlogCategoryService.Command;
using Blogs.Application.Contract.BlogCategoryService.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Users.Domain.User.Agg;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.BlogCategories
{
    public class EditModel : PageModel
    {
        private readonly IBlogCategoryCommandService _blogCategoryService;
        private readonly IBlogCategoryQueryService _blogCategoryQueryService;

        public EditModel(IBlogCategoryCommandService blogCategoryService, IBlogCategoryQueryService blogCategoryQueryService)
        {
            _blogCategoryService = blogCategoryService;
            _blogCategoryQueryService = blogCategoryQueryService;
        }

        [BindProperty]
        public EditBlogCategoryCommand EditBlogCategory { get; set; }
        public async Task<IActionResult> OnGet(int id)
        {
            
            EditBlogCategory = await _blogCategoryQueryService.GetForEditAsync(id);
              
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if(!ModelState.IsValid)
                return Page();

            var result= await _blogCategoryService.Edit(EditBlogCategory);
            if (result.Success)
            {
                TempData["success"] = "عملیات ویرایش با موفقیت انجام شد";
                return RedirectToPage("Index", new { Id = EditBlogCategory.Parent });
            }

            ModelState.AddModelError($"EditBlogCategory.{result.ModelName}", result.Message);
            return Page();

        }
    }
}
