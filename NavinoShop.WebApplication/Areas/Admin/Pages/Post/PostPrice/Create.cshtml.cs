using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PostModule.Application.Contract.PostApplication;
using PostModule.Application.Contract.PostPriceApplication;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Post.PostPrice
{
    public class CreateModel : PageModel
    {
        private readonly IPostPriceApplication _postPriceService;

        public CreateModel(IPostPriceApplication postPriceService)
        {
            _postPriceService = postPriceService;
        }
        [BindProperty]
        public CreatePostPrice CreatePostPrice { get; set; }
        public void OnGet(int postId)
        {
            CreatePostPrice = new CreatePostPrice { PostId = postId };
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _postPriceService.CreateAsync(CreatePostPrice);
            if (result.Success)
            {
                TempData["success"] = "پست با موفقیت ایجاد شد";
                return Redirect($"/Admin/Post/PostPrice/Index?id={CreatePostPrice.PostId}");
            }
            ModelState.AddModelError($"CreatePostPrice.{result.ModelName}", result.Message);
            return Page();
        }
    }
}
