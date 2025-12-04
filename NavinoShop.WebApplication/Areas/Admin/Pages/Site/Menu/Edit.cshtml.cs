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
        public async Task<IActionResult> OnGet(int Id)
        {
            if (Id < 1)
                return RedirectToPage("Index");
            Editmenu = await _menuService.GetForEditAsync(Id);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _menuService.EditAsync(Editmenu);
            if (result.Success)
            {
                TempData["success"] = "منو با موفقیت ویرایش شد";
                return Redirect($"/Admin/Site/menu/Index?parentId={Editmenu.ParentId}");
            }
            ModelState.AddModelError($"Editmenu.{result.ModelName}", result.Message);
            return Page();
        }
    }
}
