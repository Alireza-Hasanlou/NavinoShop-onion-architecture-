
using Emails.Domailn.MessageUserAgg;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NavinoShop.WebApplication.Utility;
using Query.Contract.Admin.Email.MessageUser;
using Shared.Domain.Enums;
using Users.Application.Contract.RoleService.Query;

namespace NavinoShop.WebApplication.Areas.Admin.Pages
{
    //[PermissionChecker(1)]
    public class IndexModel : PageModel
    {
        private readonly IMessageUserAdminQuery _messageUserAdminQuery;

        public IndexModel(IMessageUserAdminQuery messageUserAdminQuery)
        {
            _messageUserAdminQuery = messageUserAdminQuery;
        }
        public List<UnseenUsersMessageQueryModel> Messages { get; set; }
        public async Task<IActionResult> OnGet()
        {
            Messages = await _messageUserAdminQuery.GetUnseenUsersMessageForIndexAsync();
            return Page();
        }

    }
}
