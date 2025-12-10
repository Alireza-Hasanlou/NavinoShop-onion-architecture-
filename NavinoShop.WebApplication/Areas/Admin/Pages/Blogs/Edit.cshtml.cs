using Blogs.Application.Contract.BlogCategoryService.Query;
using Blogs.Application.Contract.BlogService.Command;
using Blogs.Application.Contract.BlogService.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Blogs
{
    public class EditModel : PageModel
    {
        private readonly IBlogCommandService _blogCommandService;
        private readonly IBlogQueryService _blogQueryService;
        private readonly IBlogCategoryQueryService _blogCategoryQueryService;

        public EditModel(IBlogCommandService blogCommandService, IBlogQueryService blogQueryService, IBlogCategoryQueryService blogCategoryQueryService)
        {
            _blogCommandService = blogCommandService;
            _blogQueryService = blogQueryService;
            _blogCategoryQueryService = blogCategoryQueryService;
        }
        [BindProperty]
        public EditBlogQueryModel EditBlog { get; set; }
        public async Task<IActionResult> OnGet(int blogId)
        {
            EditBlog = await _blogQueryService.GetForEditAsync(blogId);
            var Categories = await _blogCategoryQueryService.GetCategoriesForAddBlogAsync(0);
            ViewData["Categories"] = Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Title }).ToList();
            var subCategories = await _blogCategoryQueryService.GetCategoriesForAddBlogAsync(EditBlog.CategoryId);
            ViewData["subCategories"] = subCategories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Title, Selected = c.Id == EditBlog.SubCategoryId }).ToList();
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                var Categories = await _blogCategoryQueryService.GetCategoriesForAddBlogAsync(0);
                ViewData["Categories"] = Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Title }).ToList();

                if (EditBlog.CategoryId > 0)
                {
                    var subCategories = await _blogCategoryQueryService.GetCategoriesForAddBlogAsync(EditBlog.CategoryId);
                    ViewData["subCategories"] = subCategories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Title, Selected = c.Id == EditBlog.SubCategoryId }).ToList();
                }

                return Page();
            }

            var result = await _blogCommandService.EditAsync(EditBlog);
            if (result.Success)
            {
                TempData["success"] = "مقاله با موفقیت ویرایش شد";
                return RedirectToPage("Index", new { Id = EditBlog.CategoryId });
            }
            else
            {
                ModelState.AddModelError($"EditBlog.{result.ModelName}", result.Message);
                var Categories = await _blogCategoryQueryService.GetCategoriesForAddBlogAsync(0);
                ViewData["Categories"] = Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Title }).ToList();
                if (EditBlog.CategoryId > 0)
                {
                    var subCategories = await _blogCategoryQueryService.GetCategoriesForAddBlogAsync(EditBlog.CategoryId);
                    ViewData["subCategories"] = subCategories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Title, Selected = c.Id == EditBlog.SubCategoryId }).ToList();
                }
                return Page();
            }



        }

    }
}
