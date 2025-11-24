using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Application.Contract.BanerService.Command;
namespace NavinoShop.WebApplication.Areas.Admin.Pages.Site.Baners
{

    public class CreateModel : PageModel
    {
        private readonly IBanerCommandService _banerCommandService;

        public CreateModel(IBanerCommandService banerCommandService)
        {
            _banerCommandService = banerCommandService;
        }

        [BindProperty]
        public CreateBanerCommandModel CreateBaner { get; set; }
        public void OnGet()
        {
         
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                Page();

            var result = await _banerCommandService.CreateAsync(CreateBaner);
            if (result.Success)
            {
                TempData["Success"] = "افرودن بنر جدید با موفقیت انجام شد";
                return RedirectToPage("Index");
            }
            ModelState.AddModelError($"CreateBaner.{result.ModelName}", result.Message);
            return Page();
        }


    }
}
