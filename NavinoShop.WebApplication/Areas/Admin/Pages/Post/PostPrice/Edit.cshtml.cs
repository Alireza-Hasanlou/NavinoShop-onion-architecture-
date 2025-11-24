using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PostModule.Application.Contract.PostApplication;
using PostModule.Application.Contract.PostPriceApplication;
using PostModule.Application.Contract.PostQuery;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Post.PostPrice
{

    public class EditModel : PageModel
    {
        private readonly IPostPriceApplication _postPriceService;

        public EditModel(IPostPriceApplication postPriceService)
        {
            _postPriceService = postPriceService;
        }
        [BindProperty]
        public EditPostPrice EditPostPrice { get; set; }
        public async Task OnGet(int id)
        {
            EditPostPrice = await _postPriceService.GetForEditAsync(id);
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _postPriceService.EditAsync(EditPostPrice);
            if (result.Success)
            {
                TempData["success"] = "قیمت جدید با موفقیت ویرایش شد";
                return Redirect($"/Admin/Post/PostPrice/Index?id={EditPostPrice.PostId}");
            }
            ModelState.AddModelError($"EditPostPrice.{result.ModelName}",result.Message);
            return Page();
        }
    }
}
