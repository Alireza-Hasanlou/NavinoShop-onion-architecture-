
using Emails.Domailn.MessageUserAgg;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NavinoShop.WebApplication.Models;
using NavinoShop.WebApplication.Utility;
using Query.Contract.Admin.Email.MessageUser;
using Query.Contract.Admin.Order;
using Shared.Domain.Enums;
using Users.Application.Contract.RoleService.Query;

namespace NavinoShop.WebApplication.Areas.Admin.Pages
{
    //[PermissionChecker(1)]
    public class IndexModel : PageModel
    {
        private readonly IMessageUserAdminQuery _messageUserAdminQuery;
        private readonly IOrderAdminQueryService _orderAdminQueryService;

        public IndexModel(IMessageUserAdminQuery messageUserAdminQuery, IOrderAdminQueryService orderAdminQueryService)
        {
            _messageUserAdminQuery = messageUserAdminQuery;
            _orderAdminQueryService = orderAdminQueryService;
        }

        public AdminIndexPageViewModel Index { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var model = new AdminIndexPageViewModel();
            model.Messages = await _messageUserAdminQuery.GetUnseenUsersMessageForIndexAsync();
            model.LatestOrders = await _orderAdminQueryService.GetLatestOrdersForIndexPageAsync();
            Index= model;
            return Page();
        }

    }
}
