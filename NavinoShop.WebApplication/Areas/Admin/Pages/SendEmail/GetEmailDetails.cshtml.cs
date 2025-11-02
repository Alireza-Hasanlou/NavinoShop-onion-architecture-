using Emails.Application.Contract.SendEmailService.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NavinoShop.WebApplication.Areas.Admin.Pages.SendEmail
{
    public class GetEmailDetailsModel : PageModel
    {
        private readonly ISendEmailQueryService _sendEmailQueryService;

        public GetEmailDetailsModel(ISendEmailQueryService sendEmailQueryService)
        {
            _sendEmailQueryService = sendEmailQueryService;
        }
        public SendEmailDetailQueryModel EmailDetails { get; set; }
        public async Task<IActionResult> OnGet(int emailId)
        {
            if (emailId == 0)
                return RedirectToPage("Index");

            EmailDetails = await _sendEmailQueryService.GetSendEmailDetailForAdmin(emailId);
            return Page();
        }
    }
}
