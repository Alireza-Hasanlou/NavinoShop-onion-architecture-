
using Emails.Domailn.EmailAgg;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Query.Contract.Admin.Email.EmailUser;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.UsersEmail
{
    public class IndexModel : PageModel
    {
        private readonly IEmailAdminQuery _emailAdminQuery;


        public IndexModel(IEmailAdminQuery emailAdminQuery)
        {
            _emailAdminQuery = emailAdminQuery;
        }
        [BindProperty]
        public EmailUserAdminPaging EmailUser { get; set; }
        public async Task OnGet(int pageId, int take, string? filter = "")
        {
            EmailUser = await _emailAdminQuery.GetAllEmailForAdmin(pageId, take, filter);
        }
    }
}
