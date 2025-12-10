using Blogs.Application.Contract.BlogCategoryService.Query;
using Blogs.Application.Contract.BlogService.Command;
using Blogs.Application.Contract.BlogService.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared.Application.Auth;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Blogs
{
    public class CreateModel : PageModel
    {
        private readonly IBlogCommandService _blogCommandService;
        private readonly IBlogQueryService _blogQueryService;
        private readonly IAuthService _authService;
        private readonly IBlogCategoryQueryService _blogCategoryQueryService;

        public CreateModel(IBlogCommandService blogCommandService, IBlogQueryService blogQueryService,
                           IAuthService authService, IBlogCategoryQueryService blogCategoryQueryService)
        {
            _blogCommandService = blogCommandService;
            _blogQueryService = blogQueryService;
            _authService = authService;
            _blogCategoryQueryService = blogCategoryQueryService;
        }
        [BindProperty]
        public CreateBlogCommand CreateBlog { get; set; }
        public async Task<IActionResult> OnGet(int categoryId)
        {
            CreateBlog = new()
            {
                UserId = _authService.GetLoginUserId(),
                Writer = _authService.GetLoginUserFullName(),
                CategoryId = 0,
                SubCategoryId = 0
            };
            var Categories = await _blogCategoryQueryService.GetCategoriesForAddBlogAsync(0);
            ViewData["Categories"] = Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Title }).ToList();
            return Page();
        }

        public async Task<IActionResult> Onpost(int categoryId)
        {
            
            if (!ModelState.IsValid)
            {
                var Categories = await _blogCategoryQueryService.GetCategoriesForAddBlogAsync(categoryId);
                ViewData["Categories"] = Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Title }).ToList();
                return Page();
            }
            var result = await _blogCommandService.CreateAsync(CreateBlog);
            if (result.Success)
            {
                TempData["success"] = "مقاله با موفقیت ایجاد شد";
                return RedirectToPage("Index", new { Id = CreateBlog.CategoryId });
            }
            else
            {
            var Categories = await _blogCategoryQueryService.GetCategoriesForAddBlogAsync(0);
            ViewData["Categories"] = Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Title }).ToList();
            ModelState.AddModelError($"CreateBlog.{result.ModelName}", result.Message);
            return Page();
            }

        }

        public async Task<JsonResult> OnGetSubCategories(int categoryId)
        {
            if (categoryId <= 0)
                return new JsonResult(new List<BlogCategoryForCreateBlogQueryModel>());

            var subs = await _blogCategoryQueryService.GetCategoriesForAddBlogAsync(categoryId);

            return new JsonResult(subs);
        }
    }
}
