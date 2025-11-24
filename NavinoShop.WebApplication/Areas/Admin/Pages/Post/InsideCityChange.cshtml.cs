using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PostModule.Application.Contract.PostApplication;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Post
{
    public class InsideCityChangeModel : PageModel
    {
        private readonly IPostApplication _postService;

        public InsideCityChangeModel(IPostApplication postService)
        {
            _postService = postService;
        }

        public async Task<JsonResult> OnGet(int id)
        {

            if (id < 1)
                return new JsonResult(new { success = false });

            var result = await _postService.InsideCityChangeAsync(id);
            if (result)
                return new JsonResult(new { success = true });

            return new JsonResult(new { success = false });

        }
    
    }
}
