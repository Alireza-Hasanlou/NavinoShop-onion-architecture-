using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Application.Contract.BanerService.Command;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.Site.Baners
{
    public class DeleteModel : PageModel
    {
        private readonly IBanerCommandService _commandService;

        public DeleteModel(IBanerCommandService commandService)
        {
            _commandService = commandService;
        }

        public async Task<JsonResult> OnGet(int id)
        {
            var result = await _commandService.DeleteAsync(id);
            if (result.Success)
                return new JsonResult(new { success = true, title = "بنر با موفقیت حذف شد" });

            return new JsonResult(new { success = false, errors = result.Message });

        }
    }
}
