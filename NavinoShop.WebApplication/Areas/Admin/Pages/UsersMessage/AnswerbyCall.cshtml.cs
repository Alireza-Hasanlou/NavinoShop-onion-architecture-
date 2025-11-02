using Emails.Application.Contract.MessageUserService.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.UsersMessage
{
    public class AnswerbyCallModel : PageModel
    {
        private readonly IMessageUserCommandService _messageUserCommandService;

        public AnswerbyCallModel(IMessageUserCommandService messageUserCommandService)
        {
            _messageUserCommandService = messageUserCommandService;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            if (id < 0)
            {
                TempData["Success"] = false;
                return RedirectToPage("Inedx");
            }

            var result = await _messageUserCommandService.AnswerByCall(id);
            if (result.Success)
            {
                TempData["Success"] = true;
                return RedirectToPage("Inedx");
            }
            TempData["Success"] = false;
            return RedirectToPage("Inedx");
        }
    }
}