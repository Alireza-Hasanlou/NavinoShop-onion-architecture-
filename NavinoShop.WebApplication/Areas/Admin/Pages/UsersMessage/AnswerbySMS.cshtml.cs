using Emails.Application.Contract.MessageUserService.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NavinoShop.WebApplication.Models;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.UsersMessage
{
    [IgnoreAntiforgeryToken]
    public class AnswerbySMSModel : PageModel
    {
        private readonly IMessageUserCommandService _messageUserCommandService;

        public AnswerbySMSModel(IMessageUserCommandService messageUserCommandService)
        {
            _messageUserCommandService = messageUserCommandService;
        }

        public async Task<IActionResult> OnPost([FromBody] AnswerMessageModel model)
        {
            if (model.Id < 1 || string.IsNullOrEmpty(model.Message))
            {
                return new JsonResult(new { ok = false });
            }

            var result = await _messageUserCommandService.AnsweredBySMS(model.Id, model.Message);
            if (result.Success)
            {
                return new JsonResult(new { ok = true });
            }
            return new JsonResult(new { ok = false });
        }
  
    }
}
