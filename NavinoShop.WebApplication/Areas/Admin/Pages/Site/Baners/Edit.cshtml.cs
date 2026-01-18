using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PostModule.Application.Contract.UserPostApplication.Command;
using Site.Application.Contract.BanerService.Command;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Post.Baner
{
    public class EditModel : PageModel
    {

        private readonly IBanerCommandService _banerCommandService;

        public EditModel(IBanerCommandService banerCommandService)
        {
            _banerCommandService = banerCommandService;
        }

        [BindProperty]
        public EditBanerCommandModel EditBaner { get; set; }
        public async Task<IActionResult> OnGet(int id)
        {
            EditBaner = await _banerCommandService.GetForEditAsync(id);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                Page();

            var result = await _banerCommandService.EditAsync(EditBaner);
            if (result.Success)
            {
                TempData["Success"] = "ویرایش بنر با موفقیت انجام شد";
                return RedirectToPage("Index");
            }
            ModelState.AddModelError($"EditBaner.{result.ModelName}", result.Message);
            return Page();
        }

    }
}
