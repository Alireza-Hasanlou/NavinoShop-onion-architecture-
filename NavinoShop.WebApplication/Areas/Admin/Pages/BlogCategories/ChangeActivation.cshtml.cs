using Blogs.Application.Contract.BlogCategoryService.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.BlogCategories
{
    [IgnoreAntiforgeryToken]
    public class ChangeActivationModel : PageModel
    {
        private readonly IBlogCategoryCommandService _blogCategoryService;

        public ChangeActivationModel(IBlogCategoryCommandService blogCategoryService)
        {
            _blogCategoryService = blogCategoryService;
        }

        public async Task<JsonResult> OnGet(int id)
        {
          

            if(id<1)
                return new JsonResult (new { success = false });

            var result= await _blogCategoryService.ActivationChange(id);
            if (result.Success)
                return new JsonResult(new {success=true });

            return new JsonResult(new {success=false});
            
        }
    }
}
