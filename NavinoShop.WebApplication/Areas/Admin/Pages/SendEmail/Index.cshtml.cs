using Emails.Application.Contract.EmailUserService.Command;
using Emails.Application.Contract.SendEmailService.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Query.Contract.Admin.Email.EmailUser;
using Query.Contract.Admin.Email.MessageUser;
using Shared.Domain.Enums;
using System.Threading.Tasks;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.SendEmail
{
    public class IndexModel : PageModel
    {
        private readonly ISendEmailQueryService _sendEmailQueryService;

        public IndexModel(ISendEmailQueryService sendEmailQueryService)
        {
            _sendEmailQueryService = sendEmailQueryService;
        }
        public List<SendEmailQueryModel> Emails { get; set; }
        public async Task OnGet()
        {
            Emails = await _sendEmailQueryService.GetEmailSendsForAdmin();
        }
    }
}
