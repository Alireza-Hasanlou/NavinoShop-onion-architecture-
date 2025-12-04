using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Application.Contract.MenuService.Command;
namespace NavinoShop.WebApplication.Areas.Admin.Pages.Site.Menu
{

    public class CreateModel : PageModel
    {
        private readonly IMenuCommandService _menurCommandService;

        public CreateModel(IMenuCommandService menurCommandService)
        {
            _menurCommandService = menurCommandService;
        }

        [BindProperty]
        public CreateMenuCommandModel CreateMenu { get; set; }
        public void OnGet() => Page();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                Page();

            var result = await _menurCommandService.CreateAsync(CreateMenu);
            if (result.Success)
            {
                TempData["Success"] = "افرودن منو جدید با موفقیت انجام شد";
                return RedirectToPage("Index");
            }
            ModelState.AddModelError($"CreateMenu.{result.ModelName}", result.Message);
            return Page();
        }


    }
}
