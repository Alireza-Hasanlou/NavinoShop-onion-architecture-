using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PostModule.Application.Contract.PostApplication;
using PostModule.Application.Contract.PostQuery;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Site.Post
{

    public class EditModel : PageModel
    {
        private readonly IPostQuery _postQuery;
        private readonly IPostApplication _postService;

        public EditModel(IPostQuery postQuery, IPostApplication postService)
        {
            _postQuery = postQuery;
            _postService = postService;
        }
        [BindProperty]
        public EditPost EditPost { get; set; }
        public async Task OnGet(int id)
        {
            EditPost = await _postService.GetForEdit(id);
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _postService.Edit(EditPost);
            if (result.Success)
            {
                TempData["success"] = "پست با موفقیت ویرایش شد";
                return Redirect("/Admin/Posts/Index");
            }
            ModelState.AddModelError($"EditPost.{result.ModelName}",result.Message);
            return Page();
        }
    }
}
