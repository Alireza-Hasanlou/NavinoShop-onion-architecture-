using Emails.Application.Contract.EmailUserService.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Query.Contract.Admin.Email.EmailUser;
using Query.Contract.Admin.Email.MessageUser;
using Shared.Domain.Enums;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.UsersMessage
{
    public class IndexModel : PageModel
    {
        private readonly IMessageUserAdminQuery _messageUserAdminQuery;

        public IndexModel(IMessageUserAdminQuery messageUserAdminQuery)
        {
            _messageUserAdminQuery = messageUserAdminQuery;
        }
        public MessageUserAdminPaging MessageUser { get; set; }
        public async Task OnGet(MessageStatus status, int pageId=1, int take=10, string? filter = "")
        {
            MessageUser = await _messageUserAdminQuery.GetMessagesForAdmin(status, pageId, take, filter);
        }
    }
}
