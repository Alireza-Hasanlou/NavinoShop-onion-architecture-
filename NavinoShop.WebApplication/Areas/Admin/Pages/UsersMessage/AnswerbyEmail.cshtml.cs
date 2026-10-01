using Emails.Application.Contract.MessageUserService.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NavinoShop.WebApplication.Models;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.UsersMessage
{
    [IgnoreAntiforgeryToken]
    public class AnswerbyEmailModel : PageModel
    {
        private readonly IMessageUserCommandService _messageUserCommandService;

        public AnswerbyEmailModel(
            IMessageUserCommandService messageUserCommandService)
        {
            _messageUserCommandService = messageUserCommandService;
        }

        public async Task<IActionResult> OnPost([FromBody]AnswerMessageModel model)
        {
            if (model.Id < 1 || string.IsNullOrWhiteSpace(model.Message))
            {
                return new JsonResult(new { ok = false });
            }

            var result = await _messageUserCommandService
                .AnsweredByEmail(model.Id, model.Message);

            return new JsonResult(new
            {
                ok = result.Success
            });
        }
    }
}