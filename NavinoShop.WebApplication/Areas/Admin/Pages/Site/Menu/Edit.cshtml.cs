using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Application.Contract.MenuService.Command;


namespace NavinoShop.WebApplication.Areas.Admin.Pages.Site.Menu
{
    public class EditModel : PageModel
    {
        private readonly IMenuCommandService _menuService;

        public EditModel(IMenuCommandService menuService)
        {
            _menuService = menuService;
        }

        [BindProperty]
        public EditMenuCommandModel Editmenu { get; set; }
        public async Task OnGet(int menuId)
        {
            Editmenu = await _menuService.GetForEditAsync(menuId);
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _menuService.EditAsync(Editmenu);
            if (result.Success)
            {
                TempData["success"] = "منو با موفقیت ویرایش شد";
                return Redirect("/Admin/menu/Index");
            }
            ModelState.AddModelError($"Editmenu.{result.ModelName}", result.Message);
            return Page();
        }
    }
}
