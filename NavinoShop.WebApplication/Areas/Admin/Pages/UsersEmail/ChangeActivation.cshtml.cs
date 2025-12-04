
using Emails.Application.Contract.EmailUserService.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Application.Contract.MenuService.Command;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.UsersEmail
{
    [IgnoreAntiforgeryToken]
    public class ChangeActivationModel : PageModel
    {
        private readonly IEmailUseCommandService _emailUseCommandService;

        public ChangeActivationModel(IEmailUseCommandService emailUseCommandService)
        {
            _emailUseCommandService = emailUseCommandService;
        }

        public async Task<JsonResult> OnGet(int id)
        {

            if (id < 1)
                return new JsonResult(new { success = false });

            var result = await _emailUseCommandService.ActivationChange(id);
            if (result.Success)
                return new JsonResult(new { success = true });

            return new JsonResult(new { success = false });

        }
    }
}
