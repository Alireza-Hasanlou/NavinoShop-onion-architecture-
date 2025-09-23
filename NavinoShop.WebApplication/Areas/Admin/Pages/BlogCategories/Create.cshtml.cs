using Blogs.Application.Contract.BlogCategoryService.Command;
using Blogs.Application.Contract.BlogCategoryService.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.BlogCategories
{

    public class CreateModel : PageModel
    {
        private readonly IBlogCategoryCommandService _blogCategorycommandService;
        private readonly IBlogCategoryQueryService _blogCategoryQueryService;

        public CreateModel(IBlogCategoryCommandService blogCategorycommandService, IBlogCategoryQueryService blogCategoryQueryService)
        {
            _blogCategorycommandService = blogCategorycommandService;
            _blogCategoryQueryService = blogCategoryQueryService;
        }

        [BindProperty]
        public CreateBlogCategoryCommand createBlogCategory { get; set; }
        public async Task<IActionResult> OnGet(int parentId = 0)
        {
            if (parentId > 0 && await _blogCategoryQueryService.CheckCategoryHaveParentAsync(parentId))
                return NotFound();

            createBlogCategory = new CreateBlogCategoryCommand
            {
                Parent = parentId
            };
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _blogCategorycommandService.Create(createBlogCategory);
            if (result.Success)
            {
                TempData["success"] = "افرودن دسته بندی جدید با موفقیت انجام شد";
                return RedirectToPage("Index", new {parentId=createBlogCategory.Parent});

            }

            ModelState.AddModelError("createBlogCategory.ImageFile", result.Message);
            return Page();


        }
    }
}
