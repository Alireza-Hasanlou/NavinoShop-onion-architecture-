using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Domain.Enums;
using Site.Application.Contract.MenuService.Command;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Site.Menu
{
    public class CreateSubMenuModel : PageModel
    {
        private readonly IMenuCommandService _menurCommandService;

        public CreateSubMenuModel(IMenuCommandService menurCommandService)
        {
            _menurCommandService = menurCommandService;
        }

        [BindProperty]
        public CreateSubMenuCommandModel CreateSubMenu { get; set; }
        public async Task<IActionResult> OnGet(int ParentId)
        {
            if (ParentId < 1)
                return RedirectToPage("Index");
            CreateSubMenu  = await  _menurCommandService.GetForCreateAsync(ParentId);
            return Page();
        }
      

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                Page();
       
            var result = await _menurCommandService.CreateSubAsync(CreateSubMenu);
            if (result.Success)
            {
                TempData["Success"] = "افرودن زیر منو جدید با موفقیت انجام شد";
                return Redirect($"/Admin/Site/menu/Index?parentId={CreateSubMenu.ParentId}");
            }
            ModelState.AddModelError($"CreateSubMenu.{result.ModelName}", result.Message);
            return Page();
        }
    }
}
