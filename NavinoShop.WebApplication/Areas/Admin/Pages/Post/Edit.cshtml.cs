using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PostModule.Application.Contract.PostApplication;
using PostModule.Application.Contract.PostQuery;


namespace NavinoShop.WebApplication.Areas.Admin.Pages.Post
{

    public class EditModel : PageModel
    {
      
        private readonly IPostApplication _postService;

        public EditModel(IPostApplication postService)
        {
            _postService = postService;
        }
        [BindProperty]
        public EditPost EditPost { get; set; }
        public async Task OnGet(int id)
        {
            EditPost = await _postService.GetForEditAsync(id);
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _postService.EditAsync(EditPost);
            if (result.Success)
            {
                TempData["success"] = "پست با موفقیت ویرایش شد";
                return Redirect("/Admin/Post/Index");
            }
            ModelState.AddModelError($"EditPost.{result.ModelName}",result.Message);
            return Page();
        }
    }
}
