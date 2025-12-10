using Blogs.Application.Contract.BlogCategoryService.Command;
using Blogs.Application.Contract.BlogService.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Blogs
{
    [IgnoreAntiforgeryToken]
    public class ChangeActivationModel : PageModel
    {
        private readonly IBlogCommandService _blogService;

        public ChangeActivationModel(IBlogCommandService blogService)
        {
            _blogService = blogService;
        }

        public async Task<JsonResult> OnGet(int id)
        {
          
            if(id<1)
                return new JsonResult (new { success = false });

            var result= await _blogService.ChangeActivationAsync(id);
            if (result.Success)
                return new JsonResult(new {success=true });

            return new JsonResult(new {success=false});
            
        }
    }
}
