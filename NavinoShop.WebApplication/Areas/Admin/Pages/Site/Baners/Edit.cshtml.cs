using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Application.Contract.BanerService.Command;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Site.Baners
{
    public class EditModel : PageModel
    {
        private readonly IBanerCommandService _banerService;

        public EditModel(IBanerCommandService banerService)
        {
            _banerService = banerService;
        }

        [BindProperty]
        public EditBanerCommandModel EditBaner { get; set; }
        public async Task OnGet(int BanerId)
        {
            EditBaner = await _banerService.GetForEditAsync(BanerId);
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _banerService.EditAsync(EditBaner);
            if (result.Success)
            {
                TempData["success"] = "بنر با موفقیت ویرایش شد";
                return RedirectToPage("Index");
            }
            ModelState.AddModelError($"EditBaner.{result.ModelName}", result.Message);
            return Page();
        }
    }
}
