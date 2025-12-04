using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Query.Contract.Admin.Email.MessageUser;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.UsersMessage
{
    public class DetailModel : PageModel
    {
        private readonly IMessageUserAdminQuery _messageUserAdminQuery;

        public DetailModel(IMessageUserAdminQuery messageUserAdminQuery)
        {
            _messageUserAdminQuery = messageUserAdminQuery;
        }
        public MessageUserDetailAdminQueryModel Detail { get; set; }
        public async Task OnGet(int id)
        {
            Detail = await _messageUserAdminQuery.GetMessageDetailForAdmin(id);
        }
    }
}
