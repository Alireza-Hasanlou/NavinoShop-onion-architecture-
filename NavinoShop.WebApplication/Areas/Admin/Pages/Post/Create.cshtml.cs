using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PostModule.Application.Contract.PostApplication;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Post
{
    public class CreateModel : PageModel
    {
        private readonly IPostApplication _postService;

        public CreateModel(IPostApplication postService)
        {
            _postService = postService;
        }
        [BindProperty]
        public CreatePost CreatePost { get; set; }
        public void OnGet()
        {
           
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _postService.CreateAsync(CreatePost);
            if (result.Success)
            {
                TempData["success"] = "پست با موفقیت ایجاد شد";
                return Redirect("/Admin/Post/Index");
            }
            ModelState.AddModelError($"CreatePost.{result.ModelName}", result.Message);
            return Page();
        }
    }
}
