using Blogs.Application.Contract.BlogCategoryService.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PostModule.Application.Contract.PostApplication;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Post
{
    [IgnoreAntiforgeryToken]
    public class ChangeActivationModel : PageModel
    {
        private readonly IPostApplication _postService;

        public ChangeActivationModel(IPostApplication postService)
        {
            _postService = postService;
        }

        public async Task<JsonResult> OnGet(int id)
        {

            if (id < 1)
                return new JsonResult(new { success = false });

            var result = await _postService.ChangeActivationAsync(id);
            if (result)
                return new JsonResult(new { success = true });

            return new JsonResult(new { success = false });

        }
    }
}
